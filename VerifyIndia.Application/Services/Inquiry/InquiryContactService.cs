using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.DTO.Inquiry;
using VerifyIndia.Application.DTOs;
using VerifyIndia.Application.IServices.Inquiry;
using VerifyIndia.Application.Services.Master;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.Inquiry
{
    public class InquiryContactService : IPagedService<InquiryContactDto>
    {
        private readonly IMasterRepository<Inquiry_Contact> _repository;
        private readonly IMapper _mapper;
        public InquiryContactService(IMasterRepository<Inquiry_Contact> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }
        private Expression<Func<Inquiry_Contact, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x => x.IsActive &&
                        (
                            (x.UserName != null && x.UserName.ToLower().Contains(searchTerm)) ||
                            (x.EmailId != null && x.EmailId.ToLower().Contains(searchTerm)) ||
                            (x.MobileNo != null && x.MobileNo.ToLower().Contains(searchTerm)) ||
                            (x.Message != null && x.Message.ToLower().Contains(searchTerm))
                        );
        }

        // Define which field to check for duplicates
        private Func<IQueryable<Inquiry_Contact>, IOrderedQueryable<Inquiry_Contact>> BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "name" => q => isAsc ? q.OrderBy(x => x.UserName) : q.OrderByDescending(x => x.UserName),
                "email" => q => isAsc ? q.OrderBy(x => x.EmailId) : q.OrderByDescending(x => x.EmailId),
                "mobile" => q => isAsc ? q.OrderBy(x => x.MobileNo) : q.OrderByDescending(x => x.MobileNo),
                "message" => q => isAsc ? q.OrderBy(x => x.Message) : q.OrderByDescending(x => x.Message),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }

        public async Task<(int total, List<InquiryContactDto> data)> GetPagedAsync(DataTableRequest request)
        {
            Expression<Func<Inquiry_Contact, bool>>? filter = null;

            if (!string.IsNullOrWhiteSpace(request.Search))
                filter = BuildSearchFilter(request.Search.Trim());

            var orderBy = BuildSortExpression(request.SortColumn, request.SortDirection);

            var result = await _repository.GetPagedAsync(filter, request, orderBy);

            int total = result.TotalCount;
            List<Inquiry_Contact> products = result.Items.ToList();

            var dtos = products.Select(p => _mapper.Map<InquiryContactDto>(p)).ToList();

            return (total, dtos);
        }

        public async Task UpdateStatusAsync(string uuid, string newStatus)
        {
            var inquiry = await _repository.GetByUuidAsync(uuid);
            if (inquiry == null)
            {
                throw new KeyNotFoundException("Inquiry not found.");
            }

            inquiry.Status = newStatus;
            await _repository.UpdateAsync(inquiry);
        }
    }
}
