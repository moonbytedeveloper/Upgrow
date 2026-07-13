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
    public class WebsiteCategoryServiceRepository : MasterRepositoryBase<Website_VerificationService>
    {
        public WebsiteCategoryServiceRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<PagedResult<Website_VerificationService>> GetPagedAsync(
            Expression<Func<Website_VerificationService, bool>>? filter,
            PaginationParams pagination,
            Func<IQueryable<Website_VerificationService>, IOrderedQueryable<Website_VerificationService>>? orderBy = null,
            Func<IQueryable<Website_VerificationService>, IQueryable<Website_VerificationService>>? queryModifier = null)
        {
            // Join states with countries and map country title into CountryUUID field
            // This approach is identical to MasterFAQRepository
            return await base.GetPagedAsync(
                filter,
                pagination,
                orderBy,
                query => from s in _context.Website_VerificationService
                         join c in _context.Website_VerificationServiceCategory
                             on s.ServiceCategoryUUID equals c.UUID into websiteGroup
                         from c in websiteGroup.DefaultIfEmpty()
                         select new Website_VerificationService
                         {
                             Id = s.Id,
                             UUID = s.UUID,
                             ServiceCategoryUUID = c != null ? c.Title : s.ServiceCategoryUUID, 
                             Title = s.Title,
                             IsActive = s.IsActive
                         });
        }
    }
}