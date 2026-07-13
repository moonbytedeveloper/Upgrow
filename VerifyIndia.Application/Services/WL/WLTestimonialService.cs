using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Upgrow.Application.Commands.WL;
using Upgrow.Application.DTO.DropDown;
using Upgrow.Application.DTO.WL;
using Upgrow.Application.DTOs;
using Upgrow.Application.IServices.WL;
using Upgrow.Application.Services.Master;
using Upgrow.Domain.Common;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;
using static System.Net.Mime.MediaTypeNames;



namespace Upgrow.Application.Services.WL
{
    public class WLTestimonialService : WLBaseService<WL_MasterTestimonial, WLTestimonialDto, WLTestimonialCommand>, IWLTestimonialService
    {

        public WLTestimonialService(
            IMasterRepository<WL_MasterTestimonial> repository,
            IMasterRepository<Tenant> tenantRepository,
            IMapper mapper)
            : base(repository, tenantRepository, mapper)
        {

        }

        protected override Expression<Func<WL_MasterTestimonial, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            return x =>
                 (x.CustomerName != null && x.CustomerName.ToLower().Contains(searchTerm)) ||
                  (x.TenantName != null && x.TenantName.ToLower().Contains(searchTerm)) ||
                 (x.Comment != null && x.Comment.ToLower().Contains(searchTerm));
        }
        

        protected override async Task<bool> IsDuplicateAsync(WLTestimonialCommand command)
        {
            return await _repository.ExistsAsync(x =>
               x.CustomerName!.ToLower().Trim() == command.CustomerName.ToLower().Trim() &&
               x.TenantId == command.TenantId &&
               x.UUID != command.UUID);
            //var nameToCheck = command.CustomerName?.Trim() ?? string.Empty;

            //// Compare trimmed customer name and tenant id. Avoid ToLower() inside the expression to prevent MemoryExtensions overload ambiguity.
            //return await _repository.ExistsAsync(x =>
            //    x.CustomerName != null &&
            //    x.CustomerName.Trim() == nameToCheck &&
            //    x.TenantId == command.TenantId &&
            //    x.UUID != command.UUID);
        }

        protected override Func<IQueryable<WL_MasterTestimonial>, IOrderedQueryable<WL_MasterTestimonial>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "customerName" => q => isAsc ? q.OrderBy(x => x.CustomerName) : q.OrderByDescending(x => x.CustomerName),
                "tenantName" => q => isAsc ? q.OrderBy(x => x.TenantName) : q.OrderByDescending(x => x.TenantName),
                "companyName" => q => isAsc ? q.OrderBy(x => x.CompanyName) : q.OrderByDescending(x => x.CompanyName),
                "comment" => q => isAsc ? q.OrderBy(x => x.Comment) : q.OrderByDescending(x => x.Comment),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }
    }
}