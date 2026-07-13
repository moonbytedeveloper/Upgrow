using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities;
using Upgrow.Domain.Common;
using Upgrow.Domain.IRepositories.Master;

namespace Upgrow.Infrastructure.Repositories.Master
{
    public class MasterMenuRepository : MasterRepositoryBase<Master_Menu>, IMasterMenuRepository
    {
        public MasterMenuRepository(AppDbContext context) : base(context) { }

        public override async Task<PagedResult<Master_Menu>> GetPagedAsync(
    Expression<Func<Master_Menu, bool>>? filter,
    PaginationParams pagination,
    Func<IQueryable<Master_Menu>, IOrderedQueryable<Master_Menu>>? orderBy = null,
    Func<IQueryable<Master_Menu>, IQueryable<Master_Menu>>? queryModifier = null)
        {
            return await base.GetPagedAsync(
                filter,
                pagination,
                orderBy,
                query => from menu in _context.Master_Menu
                         join perm in _context.Master_Permission
                             on menu.PermissionUUID equals perm.UUID into permGroup
                         from perm in permGroup.DefaultIfEmpty()   // LEFT JOIN
                         select new Master_Menu
                         {
                             Id = menu.Id,
                             UUID = menu.UUID,
                             MenuName = menu.MenuName,
                             MenuLevel = menu.MenuLevel,
                             Url = menu.Url,
                             IsParent = menu.IsParent,
                             IsActive = menu.IsActive,

                             // 🔥 show permission name in UI field
                             PermissionUUID = perm != null ? perm.Name : ""
                         });
        }
        public async Task<List<Master_Menu>> GetMainParentAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .Where(x => x.IsActive==true && x.IsParent ==true && x.MenuLevel == 1)
                .OrderBy(x => x.Sequence)
                .ToListAsync();
        }
        public async Task<List<Master_Menu>> GetSubParentAsync(string mainParentUuid)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(x => x.IsActive == true && x.IsParent == true && x.MainParentUUID == mainParentUuid && x.MenuLevel == 2)
                .OrderBy(x => x.Sequence)
                .ToListAsync();
        }


    }
}
