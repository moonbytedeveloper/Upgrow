using System.Linq.Expressions;
using Upgrow.Domain.Common;
using Upgrow.Domain.Entities.Inquiry;
using Upgrow.Domain.IRepositories.Inquiry;
using Upgrow.Infrastructure.Repositories;

namespace Upgrow.Infrastructure.Repositories.Inquiry
{
    public class InquiryDistributorRepository : MasterRepositoryBase<Inquiry_Distributor>, IInquiryDistributorRepository
    {
        public InquiryDistributorRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<PagedResult<Inquiry_Distributor>> GetPagedAsync(
            Expression<Func<Inquiry_Distributor, bool>>? filter,
            PaginationParams pagination,
            Func<IQueryable<Inquiry_Distributor>, IOrderedQueryable<Inquiry_Distributor>>? orderBy = null,
            Func<IQueryable<Inquiry_Distributor>, IQueryable<Inquiry_Distributor>>? queryModifier = null)
        {
            return await base.GetPagedAsync(
                filter,
                pagination,
                orderBy,
                query => from inquiry in _context.Set<Inquiry_Distributor>()
                         join employee in _context.Master_Employee
                             on inquiry.ActionTakenBy equals employee.UUID into employeeGroup
                         from employee in employeeGroup.DefaultIfEmpty()
                         join state in _context.Master_State
                             on inquiry.StateUUID equals state.UUID into stateGroup
                         from state in stateGroup.DefaultIfEmpty()
                         join city in _context.Master_City
                             on inquiry.CityUUID equals city.UUID into cityGroup
                         from city in cityGroup.DefaultIfEmpty()
                         select new Inquiry_Distributor
                         {
                             Id = inquiry.Id,
                             UUID = inquiry.UUID,
                             FullName = inquiry.FullName,
                             EmailId = inquiry.EmailId,
                             PhoneNo = inquiry.PhoneNo,
                             CompanyName = inquiry.CompanyName,
                             BusinessType = inquiry.BusinessType,
                             StateUUID = state != null ? state.Title : inquiry.StateUUID,
                             CityUUID = city != null ? city.Title : inquiry.CityUUID,
                             ExpectedRetailers = inquiry.ExpectedRetailers,
                             Message = inquiry.Message,
                             Remark = inquiry.Remark,
                             IsActive = inquiry.IsActive,
                             ActionTakenBy = employee != null
                                 ? ((employee.FirstName ?? "") + " " + (employee.LastName ?? "")).Trim()
                                 : inquiry.ActionTakenBy
                         });
        }
    }
}
