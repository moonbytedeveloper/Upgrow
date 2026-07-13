using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Infrastructure.Repositories.Menu
{
    public class MenuRolePermissionRepository : MasterRepositoryBase<Menu_RolePermission>

    {
        public MenuRolePermissionRepository(AppDbContext context) : base(context)
        {
        }

        //public override async Task<PagedResult<Menu_RolePermission>> GetPagedAsync(
        //   Expression<Func<Menu_RolePermission, bool>>? filter,
        //   PaginationParams pagination,
        //   Func<IQueryable<Menu_RolePermission>, IOrderedQueryable<Menu_RolePermission>>? orderBy = null,
        //   Func<IQueryable<Menu_RolePermission>, IQueryable<Menu_RolePermission>>? queryModifier = null)
        //{
        //    // Join Menu_RolePermission with Master_Roles and Master_Permission so UI receives display names
        //    return await base.GetPagedAsync(
        //        filter,
        //        pagination,
        //        orderBy,
        //        query => from m in _context.Set<Menu_RolePermission>()
        //                 join r in _context.Master_Roles
        //                     on m.RoleUUID equals r.UUID into roleGroup
        //                 from r in roleGroup.DefaultIfEmpty()
        //                 join p in _context.Master_Permission
        //                     on m.PermissionUUID equals p.UUID into permGroup
        //                 from p in permGroup.DefaultIfEmpty()
        //                 select new Menu_RolePermission
        //                 {
        //                     // keep UUID/Id/IsActive coming from the original entity
        //                     UUID = m.UUID,
        //                     Id = m.Id,
        //                     IsActive = m.IsActive,
        //                     // project friendly names into the fields used by the UI mapping
        //                     RoleUUID = r != null ? r.Title : m.RoleUUID,
        //                     PermissionUUID = p != null ? p.Name : m.PermissionUUID
        //                 });
        //}
        ///// <summary>
        ///// Get all permissions for a role with display names (for dropdown filtering)
        ///// </summary>
        //public async Task<List<Menu_RolePermission>> GetByRoleWithDetailsAsync(string roleUUID)
        //{
        //    if (string.IsNullOrWhiteSpace(roleUUID))
        //        return new List<Menu_RolePermission>();

        //    return await (from mrp in _context.Set<Menu_RolePermission>()
        //                  join role in _context.Master_Roles
        //                      on mrp.RoleUUID equals role.UUID into roleGroup
        //                  from role in roleGroup.DefaultIfEmpty()
        //                  join permission in _context.Master_Permission
        //                      on mrp.PermissionUUID equals permission.UUID into permissionGroup
        //                  from permission in permissionGroup.DefaultIfEmpty()
        //                  where mrp.RoleUUID == roleUUID
        //                  select new Menu_RolePermission
        //                  {
        //                      Id = mrp.Id,
        //                      UUID = mrp.UUID,
        //                      RoleUUID = role != null ? role.Title : mrp.RoleUUID,
        //                      PermissionUUID = permission != null ? permission.Name : mrp.PermissionUUID,
        //                      IsActive = mrp.IsActive,
        //                      TenantId = mrp.TenantId
        //                  })
        //        .ToListAsync();
        //}
        public override async Task<PagedResult<Menu_RolePermission>> GetPagedAsync(
           Expression<Func<Menu_RolePermission, bool>>? filter,
           PaginationParams pagination,
           Func<IQueryable<Menu_RolePermission>, IOrderedQueryable<Menu_RolePermission>>? orderBy = null,
           Func<IQueryable<Menu_RolePermission>, IQueryable<Menu_RolePermission>>? queryModifier = null)
        {
            // ✅ Keep original UUIDs for proper filtering and lookups
            return await base.GetPagedAsync(
                filter,
                pagination,
                orderBy,
                query => from m in _context.Set<Menu_RolePermission>()
                         select new Menu_RolePermission
                         {
                             UUID = m.UUID,
                             Id = m.Id,
                             IsActive = m.IsActive,
                             RoleUUID = m.RoleUUID,          // ✅ Keep original UUID
                             PermissionUUID = m.PermissionUUID,  // ✅ Keep original UUID
                         });
        }

        /// <summary>
        /// Get all permissions for a role (preserving original UUIDs)
        /// </summary>
        public async Task<List<Menu_RolePermission>> GetByRoleWithDetailsAsync(string roleUUID)
        {
            if (string.IsNullOrWhiteSpace(roleUUID))
                return new List<Menu_RolePermission>();

            return await (from mrp in _context.Set<Menu_RolePermission>()
                          where mrp.RoleUUID == roleUUID
                          select new Menu_RolePermission
                          {
                              Id = mrp.Id,
                              UUID = mrp.UUID,
                              RoleUUID = mrp.RoleUUID,          // ✅ Keep original UUID
                              PermissionUUID = mrp.PermissionUUID,  // ✅ Keep original UUID
                              IsActive = mrp.IsActive
                          })
                .ToListAsync();
        }
    }
}