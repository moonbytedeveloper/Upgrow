using System.Linq.Expressions;
using Upgrow.Domain.Common;
using Upgrow.Domain.Entities.Inquiry;
using Upgrow.Domain.IRepositories.Inquiry;
using Upgrow.Infrastructure.Repositories;

namespace Upgrow.Infrastructure.Repositories.Inquiry
{
    public class InquiryAgentRepository : MasterRepositoryBase<Inquiry_Agent>, IInquiryAgentRepository
    {
        public InquiryAgentRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<PagedResult<Inquiry_Agent>> GetPagedAsync(
            Expression<Func<Inquiry_Agent, bool>>? filter,
            PaginationParams pagination,
            Func<IQueryable<Inquiry_Agent>, IOrderedQueryable<Inquiry_Agent>>? orderBy = null,
            Func<IQueryable<Inquiry_Agent>, IQueryable<Inquiry_Agent>>? queryModifier = null)
        {
            return await base.GetPagedAsync(
                filter,
                pagination,
                orderBy,
                query => from inquiry in _context.Set<Inquiry_Agent>()
                         join employee in _context.Master_Employee
                             on inquiry.ActionTakenBy equals employee.UUID into employeeGroup
                         from employee in employeeGroup.DefaultIfEmpty()
                         join state in _context.Master_State
                             on inquiry.StateUUID equals state.UUID into stateGroup
                         from state in stateGroup.DefaultIfEmpty()
                         join city in _context.Master_City
                             on inquiry.CityUUID equals city.UUID into cityGroup
                         from city in cityGroup.DefaultIfEmpty()
                         select new Inquiry_Agent
                         {
                             Id = inquiry.Id,
                             UUID = inquiry.UUID,
                             FName = inquiry.FName,
                             LName = inquiry.LName,
                             MName = inquiry.MName,
                             Email = inquiry.Email,
                             SalesExperience = inquiry.SalesExperience,
                             PhoneNo = inquiry.PhoneNo,
                             SalesExperienceDescription = inquiry.SalesExperienceDescription,
                             StateUUID = state != null ? state.Title : inquiry.StateUUID,
                             CityUUID = city != null ? city.Title : inquiry.CityUUID,
                             HasExistingClients = inquiry.HasExistingClients,
                             Message = inquiry.Message,
                             Remark = inquiry.Remark,
                             IsConvertedToAgent = inquiry.IsConvertedToAgent,
                             IsStatusClosed = inquiry.IsStatusClosed,
                             IsActive = inquiry.IsActive,
                             ActionTakenBy = employee != null
                                 ? ((employee.FirstName ?? "") + " " + (employee.LastName ?? "")).Trim()
                                 : inquiry.ActionTakenBy
                         });
        }
    }
}
