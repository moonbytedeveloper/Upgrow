using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Common;
using Upgrow.Domain.Entities.WL;
using Upgrow.Domain.IRepositories.WL;

namespace Upgrow.Infrastructure.Repositories.WL.Master
{
    public class WLMasterEmailTemplateRepository : MasterRepositoryBase<WL_MasterEmailTemplate>, IWLMasterEmailTemplateRepository
    {
        public WLMasterEmailTemplateRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<PagedResult<WL_MasterEmailTemplate>> GetPagedAsync(
            Expression<Func<WL_MasterEmailTemplate, bool>>? filter,
            PaginationParams pagination,
            Func<IQueryable<WL_MasterEmailTemplate>, IOrderedQueryable<WL_MasterEmailTemplate>>? orderBy = null,
            Func<IQueryable<WL_MasterEmailTemplate>, IQueryable<WL_MasterEmailTemplate>>? queryModifier = null)
        {
            return await base.GetPagedAsync(
                filter,
                pagination,
                orderBy,
                query =>
                {
                    if (queryModifier != null)
                    {
                        query = queryModifier(query);
                    }

                    var joined = from e in query
                                 join t in _context.Tenant.AsNoTracking()
                                     on (decimal?)e.TenantId equals t.Id into tenantGroup
                                 from t in tenantGroup.DefaultIfEmpty()
                                 select new WL_MasterEmailTemplate
                                 {
                                     Id = e.Id,
                                     UUID = e.UUID,
                                     IsActive = e.IsActive,
                                     TenantId = e.TenantId,
                                     TenantName = t != null ? t.TenantName : string.Empty,
                                     EmailCredentialUUID = e.EmailCredentialUUID,
                                     EmailTemplateName = e.EmailTemplateName,
                                     EmailSubject = e.EmailSubject,
                                     Description = e.Description
                                 };
                    return joined;
                });
        }

        public async Task<WL_MasterEmailTemplate?> GetByIdAsync(decimal id)
        {
            return await _context.Set<WL_MasterEmailTemplate>()
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}

