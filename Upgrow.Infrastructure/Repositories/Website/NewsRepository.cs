using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.IServices;
using Upgrow.Domain.Common;
using Upgrow.Domain.Entities;

namespace Upgrow.Infrastructure.Repositories.Website
{
    public class NewsRepository : MasterRepositoryBase<News>
    {
        private readonly IDomainResolverService _domainResolverService;

        public NewsRepository(AppDbContext context, IDomainResolverService domainResolverService) : base(context)
        {
            _domainResolverService = domainResolverService;
        }

        public override async Task<PagedResult<News>> GetPagedAsync(
            Expression<Func<News, bool>>? filter,
            PaginationParams pagination,
            Func<IQueryable<News>, IOrderedQueryable<News>>? orderBy = null,
            Func<IQueryable<News>, IQueryable<News>>? queryModifier = null)
        {
            var result = await base.GetPagedAsync(
                filter,
                pagination,
                orderBy,
                query =>
                {
                    if (queryModifier != null)
                        query = queryModifier(query);

                    return from e in query
                           join r in _context.News_Category.AsNoTracking()
                               on e.NewsCategoryUUID equals r.UUID into catGroup
                           from r in catGroup.DefaultIfEmpty()
                           select new News
                           {
                               Id = e.Id,
                               UUID = e.UUID,
                               NewsCategoryUUID = r != null ? r.Title : null,
                               Title = e.Title,
                               ShortDescription = e.ShortDescription,
                               FullDescription = e.FullDescription,
                               PublishDate = e.PublishDate,
                               Image = _domainResolverService.BuildAbsoluteUrl(e.Image),
                               CardImage = _domainResolverService.BuildAbsoluteUrl(e.CardImage),
                               Location = e.Location,
                               Source = e.Source,
                               IsActive = e.IsActive,
                               IsTopStory = e.IsTopStory
                           };
                });

            return result;
        }
    }
}