using System.Linq.Expressions;
using Upgrow.Domain.Common;
using Upgrow.Domain.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace Upgrow.Infrastructure.Repositories.WL.Master
{
    public class WLTenantDomainRepository : MasterRepositoryBase<TenantDomain>
    {
        private readonly AppDbContext _context;

        public WLTenantDomainRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<PagedResult<TenantDomain>> GetPagedAsync(
            Expression<Func<TenantDomain, bool>>? filter,
            PaginationParams pagination,
            Func<IQueryable<TenantDomain>, IOrderedQueryable<TenantDomain>>? orderBy = null,
            Func<IQueryable<TenantDomain>, IQueryable<TenantDomain>>? queryModifier = null)
        {
            // Provide queryModifier that joins TenantDomain -> Tenant and maps TenantName into the entity
            return await base.GetPagedAsync(
                filter,
                pagination,
                orderBy,
                query => from d in _context.TenantDomain
                         join t in _context.Tenant
                             // Cast types so EF can translate; adjust the cast if your TenantDomain.TenantId type differs
                             on (decimal)d.TenantId equals t.Id into tenantGroup
                         from t in tenantGroup.DefaultIfEmpty()
                         select new TenantDomain
                         {
                             Id = d.Id,
                             UUID = d.UUID,
                             // Tenant_Domain.TenantId is decimal in this entity; d.TenantId may be int in the other entity
                             TenantId = (int)(decimal)d.TenantId,
                             Domain = d.Domain,
                             DomainType = d.DomainType,
                             IsActive = d.IsActive,
                             IsPrimary = d.IsPrimary,
                             TenantName = t != null ? t.TenantName : null
                         });
        }
    }
}