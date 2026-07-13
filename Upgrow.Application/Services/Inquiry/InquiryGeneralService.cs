using AutoMapper;
using System.Linq.Expressions;
using Upgrow.Application.DTO.Inquiry;
using Upgrow.Application.DTOs;
using Upgrow.Application.IServices.Inquiry;
using Upgrow.Domain.Entities.Inquiry;
using Upgrow.Domain.IRepositories.Inquiry;

namespace Upgrow.Application.Services.Inquiry
{
    public class InquiryGeneralService : IInquiryGeneralService
    {
        private readonly IInquiryGeneralRepository _repository;
        private readonly IMapper _mapper;

        public InquiryGeneralService(IInquiryGeneralRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        private Expression<Func<Inquiry_General, bool>>? BuildSearchFilter(string searchTerm)
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

        private Func<IQueryable<Inquiry_General>, IOrderedQueryable<Inquiry_General>> BuildSortExpression(
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
            Expression<Func<Inquiry_General, bool>>? filter = null;

            if (!string.IsNullOrWhiteSpace(request.Search))
                filter = BuildSearchFilter(request.Search.Trim());

            var orderBy = BuildSortExpression(request.SortColumn, request.SortDirection);

            var result = await _repository.GetPagedAsync(filter, request, orderBy);

            var dtos = result.Items.Select(x => _mapper.Map<InquiryGeneralDto>(x)).ToList();
            return (result.TotalCount, dtos);
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

        // Reused from existing IPagedService contract (used as "remark" updater here)
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