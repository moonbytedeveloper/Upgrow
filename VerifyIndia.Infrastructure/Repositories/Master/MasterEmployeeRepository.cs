using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Common;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories.Master;

namespace Upgrow.Infrastructure.Repositories.Master
{
    public class MasterEmployeeRepository : MasterRepositoryBase<Master_Employee>, IMasterEmployeeRepository
    {
        public MasterEmployeeRepository(AppDbContext context) : base(context)
        {
        }
        public override async Task<PagedResult<Master_Employee>> GetPagedAsync(
        Expression<Func<Master_Employee, bool>>? filter,
        PaginationParams pagination,
        Func<IQueryable<Master_Employee>, IOrderedQueryable<Master_Employee>>? orderBy = null,
        Func<IQueryable<Master_Employee>, IQueryable<Master_Employee>>? queryModifier = null)
        {
            return await base.GetPagedAsync(
                filter,
                pagination,
                orderBy,
                query =>
                    (from e in _context.Master_Employee

                     join r in _context.Master_Roles
                         on e.RoleUUID equals r.UUID into roleGroup
                     from r in roleGroup.DefaultIfEmpty()

                     join d in _context.Master_Department
                         on e.DepartmentUUID equals d.UUID into departmentGroup
                     from d in departmentGroup.DefaultIfEmpty()

                     join des in _context.Master_Designation
                         on e.DesignationUUID equals des.UUID into designationGroup
                     from des in designationGroup.DefaultIfEmpty()

                     join h in _context.Master_Honorific
                         on e.HonorificUUID equals h.UUID into honorificGroup
                     from h in honorificGroup.DefaultIfEmpty()

                     select new { e, r, d, des, h })
                    .GroupBy(x => x.e.UUID)
                    .Select(g => new Master_Employee
                    {
                        UUID = g.Key,

                        FirstName = g.Select(x => x.e.FirstName).FirstOrDefault(),
                        LastName = g.Select(x => x.e.LastName).FirstOrDefault(),
                        EmailId = g.Select(x => x.e.EmailId).FirstOrDefault(),
                        UserName = g.Select(x => x.e.UserName).FirstOrDefault(),
                        Password = g.Select(x => x.e.Password).FirstOrDefault(),
                        CompanyEmailID = g.Select(x => x.e.CompanyEmailID).FirstOrDefault(),
                        CompanyMobileNumber = g.Select(x => x.e.CompanyMobileNumber).FirstOrDefault(),
                        EmployeeCode = g.Select(x => x.e.EmployeeCode).FirstOrDefault(),
                        MobileNumber = g.Select(x => x.e.MobileNumber).FirstOrDefault(),
                        IsLoginAllowed = g.Select(x => x.e.IsLoginAllowed).FirstOrDefault(),
                        IsActive = g.Select(x => x.e.IsActive).FirstOrDefault(),
                        Id = g.Select(x => x.e.Id).FirstOrDefault(),

                        // Map titles from joined tables
                        RoleUUID = g.Select(x => x.r.Title).FirstOrDefault(),
                        DepartmentUUID = g.Select(x => x.d.Title).FirstOrDefault(),
                        DesignationUUID = g.Select(x => x.des.Title).FirstOrDefault(),
                        HonorificUUID = g.Select(x => x.h.Title).FirstOrDefault()
                    })
            );
        }

        public async Task<Master_Employee?> GetByCredentialsAsync(string username)
        {
            var user = await _context.Master_Employee
               .FirstOrDefaultAsync(x =>
                   x.UserName == username &&
                   x.IsActive == true &&
                   x.IsLoginAllowed == true);

            if (user == null)
                return null;

            if (!string.Equals(user.UserName, username, StringComparison.Ordinal))
                return null;

            return user;
        }

        public async Task<Master_Employee?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            var em = email.Trim().ToLower();
            return await _context.Master_Employee
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.EmailId != null && e.EmailId.ToLower() == em);
        }

        public async Task<Master_Employee?> GetByEmployeeCodeAsync(string employeeCode)
        {
            if (string.IsNullOrWhiteSpace(employeeCode))
                return null;

            var code = employeeCode.Trim();
            return await _context.Master_Employee
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.EmployeeCode == code && x.IsActive);
        }
    }


}
