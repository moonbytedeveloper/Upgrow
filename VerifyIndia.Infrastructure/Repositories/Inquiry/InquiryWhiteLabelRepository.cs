using System.Linq.Expressions;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities.Inquiry;
using VerifyIndia.Domain.IRepositories.Inquiry;
using VerifyIndia.Infrastructure.Repositories;

namespace VerifyIndia.Infrastructure.Repositories.Inquiry
{
    public class InquiryWhiteLabelRepository : MasterRepositoryBase<Inquiry_WhiteLabel>, IInquiryWhiteLabelRepository
    {
        public InquiryWhiteLabelRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<PagedResult<Inquiry_WhiteLabel>> GetPagedAsync(
            Expression<Func<Inquiry_WhiteLabel, bool>>? filter,
            PaginationParams pagination,
            Func<IQueryable<Inquiry_WhiteLabel>, IOrderedQueryable<Inquiry_WhiteLabel>>? orderBy = null,
            Func<IQueryable<Inquiry_WhiteLabel>, IQueryable<Inquiry_WhiteLabel>>? queryModifier = null)
        {
            return await base.GetPagedAsync(
                filter,
                pagination,
                orderBy,
                query => from inquiry in _context.Set<Inquiry_WhiteLabel>()
                         join employee in _context.Master_Employee
                             on inquiry.ActionTakenBy equals employee.UUID into employeeGroup
                         from employee in employeeGroup.DefaultIfEmpty()
                         join state in _context.Master_State
                             on inquiry.StateUUID equals state.UUID into stateGroup
                         from state in stateGroup.DefaultIfEmpty()
                         join city in _context.Master_City
                             on inquiry.CityUUID equals city.UUID into cityGroup
                         from city in cityGroup.DefaultIfEmpty()
                         select new Inquiry_WhiteLabel
                         {
                             Id = inquiry.Id,
                             UUID = inquiry.UUID,
                             FullName = inquiry.FullName,
                             EmailId = inquiry.EmailId,
                             PhoneNo = inquiry.PhoneNo,
                             CompanyName = inquiry.CompanyName,
                             CompanyWebsite = inquiry.CompanyWebsite,
                             StateUUID = state != null ? state.Title : inquiry.StateUUID,
                             CityUUID = city != null ? city.Title : inquiry.CityUUID,
                             BusinessType = inquiry.BusinessType,
                             PreferredDomainName = inquiry.PreferredDomainName,
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
