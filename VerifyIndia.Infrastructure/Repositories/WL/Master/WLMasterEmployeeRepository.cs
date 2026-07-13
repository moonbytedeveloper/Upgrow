using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.Entities.WL.Master;
using VerifyIndia.Domain.IRepositories.Master;
using VerifyIndia.Domain.IRepositories.WL;

namespace VerifyIndia.Infrastructure.Repositories.WL.Master
{
    public class WLMasterEmployeeRepository : MasterRepositoryBase<WL_MasterEmployee>, IWLMasterEmployeeRepository
    {
        public WLMasterEmployeeRepository(AppDbContext context) : base(context)
        {
        }
        public override async Task<PagedResult<WL_MasterEmployee>> GetPagedAsync(
     Expression<Func<WL_MasterEmployee, bool>>? filter,
     PaginationParams pagination,
     Func<IQueryable<WL_MasterEmployee>, IOrderedQueryable<WL_MasterEmployee>>? orderBy = null,
     Func<IQueryable<WL_MasterEmployee>, IQueryable<WL_MasterEmployee>>? queryModifier = null)
        {
            return await base.GetPagedAsync(
                filter,
                pagination,
                orderBy,
                query =>
                    from e in _context.WL_MasterEmployee

                    join r in _context.WL_MasterRoles
                        on e.RoleUUID equals r.UUID into roleGroup
                    from r in roleGroup.DefaultIfEmpty()

                    join d in _context.WL_MasterDepartment
                        on e.DepartmentUUID equals d.UUID into departmentGroup
                    from d in departmentGroup.DefaultIfEmpty()

                    join des in _context.WL_MasterDesignation
                        on e.DesignationUUID equals des.UUID into designationGroup
                    from des in designationGroup.DefaultIfEmpty()

                    join h in _context.WL_MasterHonorific
                        on e.HonorificUUID equals h.UUID into honorificGroup
                    from h in honorificGroup.DefaultIfEmpty()

                    select new WL_MasterEmployee
                    {
                        Id = e.Id,
                        UUID = e.UUID,

                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        EmailId = e.EmailId,
                        UserName = e.UserName,
                        Password = e.Password,
                        CompanyEmailID = e.CompanyEmailID,
                        CompanyMobileNumber = e.CompanyMobileNumber,
                        EmployeeCode = e.EmployeeCode,
                        MobileNumber = e.MobileNumber,
                        IsLoginAllowed = e.IsLoginAllowed,
                        IsActive = e.IsActive,
                        RoleUUID = r != null ? r.Title : e.RoleUUID,
                        DepartmentUUID = d != null ? d.Title : e.DepartmentUUID,
                        DesignationUUID = des != null ? des.Title : e.DesignationUUID,
                        HonorificUUID = h != null ? h.Title : e.HonorificUUID
                    }
            );
        }

        public async Task<WL_MasterEmployee?> GetByCredentialsAsync(string username)
        {
            var user = await _context.WL_MasterEmployee
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

        public async Task<WL_MasterEmployee?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            var em = email.Trim().ToLower();
            return await _context.WL_MasterEmployee
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.EmailId != null && e.EmailId.ToLower() == em);
        }
        public async Task<WL_MasterEmployee?> GetByEmployeeCodeAsync(string employeeCode)
        {
            if (string.IsNullOrWhiteSpace(employeeCode))
                return null;

            var code = employeeCode.Trim();
            return await _context.WL_MasterEmployee
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.EmployeeCode == code && x.IsActive);
        }
    }


}
