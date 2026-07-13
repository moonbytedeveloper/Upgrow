using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories.Master;

namespace VerifyIndia.Infrastructure.Repositories.Master
{
    public class WLMasterPathPermissionRepository : MasterRepositoryBase<WL_Master_PathPermission>, IWLMasterPathPermissionRepository
    {
        public WLMasterPathPermissionRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<PagedResult<WL_Master_PathPermission>> GetPagedAsync(
            Expression<Func<WL_Master_PathPermission, bool>>? filter,
            PaginationParams pagination,
            Func<IQueryable<WL_Master_PathPermission>, IOrderedQueryable<WL_Master_PathPermission>>? orderBy = null,
            Func<IQueryable<WL_Master_PathPermission>, IQueryable<WL_Master_PathPermission>>? queryModifier = null)
        {
            // Join Path and Permission to project friendly names into the PathUUID / PermissionUUID fields
            return await base.GetPagedAsync(
                filter,
                pagination,
                orderBy,
                query => from e in _context.WL_Master_PathPermission
                         join p in _context.WL_Master_Path
                             on e.PathUUID equals p.UUID into pathGroup
                         from p in pathGroup.DefaultIfEmpty()
                         join r in _context.WL_MasterPermission
                             on e.PermissionUUID equals r.UUID into permGroup
                         from r in permGroup.DefaultIfEmpty()
                         select new WL_Master_PathPermission
                         {
                             UUID = e.UUID,
                             // Map display values (title/name) into the DTO-facing fields used by the UI
                             PathUUID = p != null ? p.Path : e.PathUUID,
                             PermissionUUID = r != null ? r.Name : e.PermissionUUID,
                             IsActive = e.IsActive,
                             Id = e.Id
                         });
        }
    }
}