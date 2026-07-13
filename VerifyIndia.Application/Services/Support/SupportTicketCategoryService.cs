using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.Commands.Support;
using Upgrow.Application.DTO.Customer;
using Upgrow.Application.DTO.Support;
using Upgrow.Application.DTO.WL;
using Upgrow.Application.IServices.Support;
using Upgrow.Application.Services.Master;
using Upgrow.Application.Services.Support;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;
using Upgrow.Domain.IRepositories.Master;

namespace Upgrow.Application.Services.Support
{
    public class SupportTicketCategoryService : MasterServiceBase<Support_TicketCategory, SupportTicketCategoryDto, SupportTicketCategoryCommand>, ISupportTicketCategoryService
    {
        public SupportTicketCategoryService(
            IMapper mapper,
            IMasterRepository<Support_TicketCategory> repository
            )
          : base(repository, mapper)
        {
  
        }

        protected override async Task<bool> IsDuplicateAsync(SupportTicketCategoryCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.Title!.ToLower().Trim() == command.Title.ToLower().Trim() &&
                x.DesignationUUID!.ToLower().Trim() == command.DesignationUUID.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        protected override Expression<Func<Support_TicketCategory, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.Title != null && x.Title.ToLower().Contains(searchTerm)) ||
                (x.DesignationUUID != null && x.DesignationUUID.ToLower().Contains(searchTerm)) ||
                (x.UserType != null && x.UserType.ToLower().Contains(searchTerm));
        }

        protected override Func<IQueryable<Support_TicketCategory>, IOrderedQueryable<Support_TicketCategory>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "title" => q => isAsc ? q.OrderBy(x => x.Title) : q.OrderByDescending(x => x.Title),
                "designation" => q => isAsc ? q.OrderBy(x => x.DesignationUUID) : q.OrderByDescending(x => x.DesignationUUID),
                "usertype" => q => isAsc ? q.OrderBy(x => x.UserType) : q.OrderByDescending(x => x.UserType),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }
        public async Task<List<SupportTicketCategoryDto>> GetAllAsync(Expression<Func<Support_TicketCategory, bool>> predicate)
        {
            var entities = await _repository.GetAllActiveAsync();

            var filtered = entities.AsQueryable().Where(predicate); 

            return filtered
                .Select(t => new SupportTicketCategoryDto
                {
                    UUID = t.UUID,
                    Title = t.Title,
                    UserType = t.UserType
                }).ToList();
        }
    }
}
