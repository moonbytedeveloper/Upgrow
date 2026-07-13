using System.Linq.Expressions;
using VerifyIndia.Application.DTO.Inquiry;
using VerifyIndia.Application.DTOs;
using VerifyIndia.Application.IServices.Inquiry;
using VerifyIndia.Domain.Entities.Inquiry;
using VerifyIndia.Domain.IRepositories.Inquiry;

namespace VerifyIndia.Application.Services.Inquiry
{
    public class InquiryWhiteLabelService : IInquiryWhiteLabelService
    {
        private readonly IInquiryWhiteLabelRepository _repository;

        public InquiryWhiteLabelService(IInquiryWhiteLabelRepository repository)
        {
            _repository = repository;
        }

        private Expression<Func<Inquiry_WhiteLabel, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.FullName != null && x.FullName.ToLower().Contains(searchTerm)) ||
                (x.EmailId != null && x.EmailId.ToLower().Contains(searchTerm)) ||
                (x.PhoneNo != null && x.PhoneNo.ToLower().Contains(searchTerm)) ||
                (x.Message != null && x.Message.ToLower().Contains(searchTerm)) ||
                (x.Remark != null && x.Remark.ToLower().Contains(searchTerm)) ||
                (x.ActionTakenBy != null && x.ActionTakenBy.ToLower().Contains(searchTerm));
        }

        private Func<IQueryable<Inquiry_WhiteLabel>, IOrderedQueryable<Inquiry_WhiteLabel>> BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "fullname" => q => isAsc ? q.OrderBy(x => x.FullName) : q.OrderByDescending(x => x.FullName),
                "emailid" => q => isAsc ? q.OrderBy(x => x.EmailId) : q.OrderByDescending(x => x.EmailId),
                "phoneno" => q => isAsc ? q.OrderBy(x => x.PhoneNo) : q.OrderByDescending(x => x.PhoneNo),
                "message" => q => isAsc ? q.OrderBy(x => x.Message) : q.OrderByDescending(x => x.Message),
                "remark" => q => isAsc ? q.OrderBy(x => x.Remark) : q.OrderByDescending(x => x.Remark),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }

        public async Task<(int total, List<InquiryGeneralDto> data)> GetPagedAsync(DataTableRequest request)
        {
            Expression<Func<Inquiry_WhiteLabel, bool>>? filter = null;

            if (!string.IsNullOrWhiteSpace(request.Search))
                filter = BuildSearchFilter(request.Search.Trim());

            var orderBy = BuildSortExpression(request.SortColumn, request.SortDirection);
            var result = await _repository.GetPagedAsync(filter, request, orderBy);

            var data = result.Items.Select(x => new InquiryGeneralDto
            {
                UUID = x.UUID,
                FName = x.FullName,
                EmailId = x.EmailId,
                PhoneNo = x.PhoneNo,
                Message = x.Message,
                IsActive = x.IsActive,
                Remark = x.Remark,
                ActionTakenBy = x.ActionTakenBy,
                CompanyName = x.CompanyName,
                CompanyWebsite = x.CompanyWebsite,
                BusinessType = x.BusinessType,
                PreferredDomainName = x.PreferredDomainName,
                StateUUID = x.StateUUID,
                CityUUID = x.CityUUID
            }).ToList();

            return (result.TotalCount, data);
        }

        public async Task UpdateInquiryStatusAsync(string uuid, bool isActive, string remark, string actionTakenBy)
        {
            var inquiry = await _repository.GetByUuidAsync(uuid);
            if (inquiry == null)
                throw new KeyNotFoundException("Inquiry not found.");

            inquiry.IsActive = isActive;
            inquiry.Remark = remark;
            inquiry.ActionTakenBy = actionTakenBy;

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
    }
}
