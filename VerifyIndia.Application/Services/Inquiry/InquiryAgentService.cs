using System.Linq.Expressions;
using Upgrow.Application.DTO.Inquiry;
using Upgrow.Application.DTOs;
using Upgrow.Application.IServices.Inquiry;
using Upgrow.Domain.Entities;
using Upgrow.Domain.Entities.Inquiry;
using Upgrow.Domain.IRepositories.Inquiry;
using Upgrow.Domain.IRepositories.Master;

namespace Upgrow.Application.Services.Inquiry
{
    public class InquiryAgentService : IInquiryAgentService
    {
        private readonly IInquiryAgentRepository _repository;
        private readonly IMasterCustomerRepository _customerRepository;

        public InquiryAgentService(IInquiryAgentRepository repository,IMasterCustomerRepository customerRepository)
        {
            _repository = repository;
            _customerRepository = customerRepository;
        }

        private Expression<Func<Inquiry_Agent, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.FName != null && x.FName.ToLower().Contains(searchTerm)) ||
                (x.Email != null && x.Email.ToLower().Contains(searchTerm)) ||
                (x.PhoneNo != null && x.PhoneNo.ToLower().Contains(searchTerm)) ||
                (x.Message != null && x.Message.ToLower().Contains(searchTerm)) ||
                (x.Remark != null && x.Remark.ToLower().Contains(searchTerm)) ||
                (x.ActionTakenBy != null && x.ActionTakenBy.ToLower().Contains(searchTerm));
        }

        private Func<IQueryable<Inquiry_Agent>, IOrderedQueryable<Inquiry_Agent>> BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "fullname" => q => isAsc ? q.OrderBy(x => x.FName) : q.OrderByDescending(x => x.FName),
                "emailid" => q => isAsc ? q.OrderBy(x => x.Email) : q.OrderByDescending(x => x.Email),
                "phoneno" => q => isAsc ? q.OrderBy(x => x.PhoneNo) : q.OrderByDescending(x => x.PhoneNo),
                "message" => q => isAsc ? q.OrderBy(x => x.Message) : q.OrderByDescending(x => x.Message),
                "remark" => q => isAsc ? q.OrderBy(x => x.Remark) : q.OrderByDescending(x => x.Remark),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }

        public async Task<(int total, List<InquiryGeneralDto> data)> GetPagedAsync(DataTableRequest request)
        {
            Expression<Func<Inquiry_Agent, bool>>? filter = null;

            if (!string.IsNullOrWhiteSpace(request.Search))
                filter = BuildSearchFilter(request.Search.Trim());

            var orderBy = BuildSortExpression(request.SortColumn, request.SortDirection);
            var result = await _repository.GetPagedAsync(filter, request, orderBy);

            var data = result.Items.Select(x => new InquiryGeneralDto
            {
                UUID = x.UUID,
                FName = x.FName,
                LName = x.LName,
                MName = x.MName,
                EmailId = x.Email,
                PhoneNo = x.PhoneNo,
                Message = x.Message,
                IsActive = x.IsActive,
                Remark = x.Remark,
                ActionTakenBy = x.ActionTakenBy,
                SalesExperience = x.SalesExperience,
                SalesExperienceDescription = x.SalesExperienceDescription,
                HasExistingClients = x.HasExistingClients,
                StateUUID = x.StateUUID,
                IsConvertedToAgent = x.IsConvertedToAgent,
                IsStatusClosed = x.IsStatusClosed,
                CityUUID = x.CityUUID
            }).ToList();

            return (result.TotalCount, data);
        }

        public async Task UpdateInquiryStatusAsync(string uuid, bool isActive, string remark, string actionTakenBy)
        {
            var inquiry = await _repository.GetByUuidAsync(uuid);
            if (inquiry == null)
                throw new KeyNotFoundException("Inquiry not found.");

            inquiry.IsActive = false;
            inquiry.Remark = remark;
            inquiry.ActionTakenBy = actionTakenBy;
            inquiry.IsStatusClosed = true;
            await _repository.UpdateAsync(inquiry);
        }

        public async Task UpdateStatusAsync(string uuid, string newStatus)
        {
            var inquiry = await _repository.GetByUuidAsync(uuid);
            if (inquiry == null)
                throw new KeyNotFoundException("Inquiry not found.");

            inquiry.Remark = newStatus;
            await _repository.UpdateAsync(inquiry);
        }
        public async Task ConvertToAgentAsync(string uuid, string userId)
        {
            var inquiry = await _repository.GetByUuidAsync(uuid);

            if (inquiry == null)
                throw new Exception("Inquiry not found.");

            var customer = new Master_Customer
            {
                UUID = inquiry.UUID,
                FName = inquiry.FName,
                LName = inquiry.LName,
                MName = inquiry.MName,
                Email = inquiry.Email,
                Mobile = inquiry.PhoneNo,
                IsAgent = true,
                IsActive = true

            };

            await _customerRepository.AddAsync(customer);

            inquiry.IsActive = false;
            inquiry.IsConvertedToAgent = true;
            inquiry.Remark = "Converted to Agent";
            await _repository.UpdateAsync(inquiry);
        }
    }
}
