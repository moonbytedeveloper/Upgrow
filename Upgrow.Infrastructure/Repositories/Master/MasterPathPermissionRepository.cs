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
    public class MasterPathPermissionRepository : MasterRepositoryBase<Master_PathPermission>, IMasterPathPermissionRepository
    {
        public MasterPathPermissionRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<PagedResult<Master_PathPermission>> GetPagedAsync(
            Expression<Func<Master_PathPermission, bool>>? filter,
            PaginationParams pagination,
            Func<IQueryable<Master_PathPermission>, IOrderedQueryable<Master_PathPermission>>? orderBy = null,
            Func<IQueryable<Master_PathPermission>, IQueryable<Master_PathPermission>>? queryModifier = null)
        {
            // Join Path and Permission to project friendly names into the PathUUID / PermissionUUID fields
            return await base.GetPagedAsync(
                filter,
                pagination,
                orderBy,
                query => from e in _context.Master_PathPermission
                         join p in _context.Master_Path
                             on e.PathUUID equals p.UUID into pathGroup
                         from p in pathGroup.DefaultIfEmpty()
                         join r in _context.Master_Permission
                             on e.PermissionUUID equals r.UUID into permGroup
                         from r in permGroup.DefaultIfEmpty()
                         select new Master_PathPermission
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
    
