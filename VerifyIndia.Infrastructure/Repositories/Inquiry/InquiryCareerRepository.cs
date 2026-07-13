using System.Linq.Expressions;
using Upgrow.Domain.Common;
using Upgrow.Domain.Entities.Inquiry;
using Upgrow.Domain.IRepositories.Inquiry;
using Upgrow.Infrastructure.Repositories;

namespace Upgrow.Infrastructure.Repositories.Inquiry
{
    public class InquiryCareerRepository : MasterRepositoryBase<Inquiry_Career>, IInquiryCareerRepository
    {
        public InquiryCareerRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<PagedResult<Inquiry_Career>> GetPagedAsync(
            Expression<Func<Inquiry_Career, bool>>? filter,
            PaginationParams pagination,
            Func<IQueryable<Inquiry_Career>, IOrderedQueryable<Inquiry_Career>>? orderBy = null,
            Func<IQueryable<Inquiry_Career>, IQueryable<Inquiry_Career>>? queryModifier = null)
        {
            return await base.GetPagedAsync(
                filter,
                pagination,
                orderBy,
                query => from inquiry in _context.Set<Inquiry_Career>()
                         join employee in _context.Master_Employee
                             on inquiry.ActionTakenBy equals employee.UUID into employeeGroup
                         from employee in employeeGroup.DefaultIfEmpty()
                         select new Inquiry_Career
                         {
                             Id = inquiry.Id,
                             UUID = inquiry.UUID,
                             FullName = inquiry.FullName,
                             Email = inquiry.Email,
                             PhoneNo = inquiry.PhoneNo,
                             Message = inquiry.Message,
                             Remark = inquiry.Remark,
                             IsActive = inquiry.IsActive,
                             JobPosition = inquiry.JobPosition,
                             Experience = inquiry.Experience,
                             Qualification = inquiry.Qualification,
                             Resume = inquiry.Resume,
                             ActionTakenBy = employee != null
                                 ? ((employee.FirstName ?? "") + " " + (employee.LastName ?? "")).Trim()
                                 : inquiry.ActionTakenBy
                         });
        }
    }
}
