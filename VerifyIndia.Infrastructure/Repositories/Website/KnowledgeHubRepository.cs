using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.IServices;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Infrastructure.Repositories.Website
{
    public class KnowledgeHubRepository : MasterRepositoryBase<Knowledge_Hub>
    {
        private readonly IDomainResolverService _domainResolverService;
        public KnowledgeHubRepository(
            IDomainResolverService domainResolverService,
            AppDbContext context
            ) : base(context)
        {
            _domainResolverService = domainResolverService;
        }
        public override async Task<PagedResult<Knowledge_Hub>> GetPagedAsync(
    Expression<Func<Knowledge_Hub, bool>>? filter,
    PaginationParams pagination,
    Func<IQueryable<Knowledge_Hub>, IOrderedQueryable<Knowledge_Hub>>? orderBy = null,
    Func<IQueryable<Knowledge_Hub>, IQueryable<Knowledge_Hub>>? queryModifier = null)
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

                    return from e in query
                           join c in _context.Knowledge_Hub_Category.AsNoTracking()
                               on e.CategoryUUID equals c.UUID into categoryGroup
                           from c in categoryGroup.DefaultIfEmpty()
                           select new Knowledge_Hub
                           {
                               UUID = e.UUID,
                               Name = e.Name,
                               Description = e.Description,
                               LongDescription = e.LongDescription,
                               ImageURL = _domainResolverService.BuildAbsoluteUrl(e.ImageURL),
                               IsActive = e.IsActive,
                               CategoryUUID = c != null ? c.Name : null,
                               Id = e.Id,
                               SequenceNo = e.SequenceNo
                           };
                });
        }
        //public override async Task<List<Knowledge_Hub>> GetAllActiveAsync()
        //    => await _dbSet.AsNoTracking().ToListAsync();

    }
}
