using System.Linq.Expressions;
using Upgrow.Domain.Common;
using Upgrow.Domain.Entities;
using Upgrow.Domain.Entities.Inquiry;
using Upgrow.Domain.IRepositories.Inquiry;
using Upgrow.Infrastructure.Repositories;

namespace Upgrow.Infrastructure.Repositories.Inquiry
{
    public class InquiryGeneralRepository : MasterRepositoryBase<Inquiry_General>, IInquiryGeneralRepository
    {
        public InquiryGeneralRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<PagedResult<Inquiry_General>> GetPagedAsync(
            Expression<Func<Inquiry_General, bool>>? filter,
            PaginationParams pagination,
            Func<IQueryable<Inquiry_General>, IOrderedQueryable<Inquiry_General>>? orderBy = null,
            Func<IQueryable<Inquiry_General>, IQueryable<Inquiry_General>>? queryModifier = null)
        {
            return await base.GetPagedAsync(
                filter,
                pagination,
                orderBy,
                query => from inquiry in _context.Set<Inquiry_General>()
                         join employee in _context.Master_Employee
                             on inquiry.ActionTakenBy equals employee.UUID into employeeGroup
                         from employee in employeeGroup.DefaultIfEmpty()
                         select new Inquiry_General
                         {
                             Id = inquiry.Id,
                             UUID = inquiry.UUID,
                             FullName = inquiry.FullName,
                             EmailId = inquiry.EmailId,
                             PhoneNo = inquiry.PhoneNo,
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