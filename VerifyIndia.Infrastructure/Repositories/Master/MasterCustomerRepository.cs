using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.Entities.Auth;
using VerifyIndia.Domain.IRepositories.Master;

namespace VerifyIndia.Infrastructure.Repositories.Master
{
    public class MasterCustomerRepository : MasterRepositoryBase<Master_Customer>, IMasterCustomerRepository
    {
        public MasterCustomerRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Master_Customer?> GetByAadhaarHashAsync(
            string aadhaarHash)
        {
            return await _context.Master_Customer
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.IsActive
                    &&
                    x.AadhaarHash == aadhaarHash);
        }

        public async Task<Master_Customer?> GetReferralAgentAsync(
            string mobile)
        {
            return await _context
                .Master_Customer
                .FirstOrDefaultAsync(x =>
                    x.Mobile == mobile &&
                    x.IsActive &&
                    x.IsAccessAllowed &&
                    (
                        x.IsAgent ||
                        x.IsAgentHead
                    ));
        }

        public async Task<Master_Customer?> GetByMobileAndClientIdAsync(string mobile, decimal clientId)
        {
            if (string.IsNullOrWhiteSpace(mobile))
                return null;

            var normalizedMobile = mobile.Trim();          

            return await _context.Master_Customer
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.IsActive &&
                    x.TenantId == clientId &&
                    x.Mobile == normalizedMobile);
        }

        public async Task<Master_Customer?> GetByMobileAsync(string mobile)
        {
            if (string.IsNullOrWhiteSpace(mobile))
                return null;

            var normalizedMobile = mobile.Trim();          

            return await _context.Master_Customer
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.IsActive &&                    
                    x.Mobile == normalizedMobile);
        }

        public async Task<bool> ExistsByUuidAndClientIdAsync(string customerUuid, string clientId)
        {
            if (string.IsNullOrWhiteSpace(customerUuid) || string.IsNullOrWhiteSpace(clientId))
                return false;

            if (!decimal.TryParse(clientId, out decimal clientIdDecimal))
                return false;

            return await _context.Master_Customer
                .AsNoTracking()
                .AnyAsync(x => x.IsActive && x.UUID == customerUuid && x.TenantId == clientIdDecimal);
        }
        public override async Task<PagedResult<Master_Customer>> GetPagedAsync(
        Expression<Func<Master_Customer, bool>>? filter,
       PaginationParams pagination,
        Func<IQueryable<Master_Customer>, IOrderedQueryable<Master_Customer>>? orderBy = null,
        Func<IQueryable<Master_Customer>, IQueryable<Master_Customer>>? queryModifier = null)
        {
            return await base.GetPagedAsync(
                  filter,
                pagination,
                orderBy, query => from e in _context.Master_Customer
                                  join c in _context.Master_City
                                      on e.CityUUID equals c.UUID into cityGroup
                                  from c in cityGroup.DefaultIfEmpty()
                                  join i in _context.Master_Industry
                                      on e.IndustryUUID equals i.UUID into industryGroup
                                  from i in industryGroup.DefaultIfEmpty()
                                  select new Master_Customer
                                  {
                                      UUID = e.UUID,
                                      FName = e.FName,
                                      MName = e.MName,
                                      LName = e.LName,
                                      Mobile = e.Mobile,
                                      Email = e.Email,
                                      ACType = e.ACType,
                                      IsAgent = e.IsAgent,
                                      IsAgentHead = e.IsAgentHead,
                                      ACLink = e.ACLink,
                                      TenantId = e.TenantId,
                                      ReferralCode = e.ReferralCode,
                                      IsRegistered = e.IsRegistered,
                                      IsAccessAllowed = e.IsAccessAllowed,
                                      RegTimeStamp = e.RegTimeStamp,
                                      IsRegisteredAsX = e.IsRegisteredAsX,
                                      XApiKey = e.XApiKey,
                                      CurrentStep = e.CurrentStep,
                                      IsActive = e.IsActive,
                                      CityUUID = c != null ? c.Title : null,       // map title
                                      IndustryUUID = i != null ? i.Title : null,       // map title
                                      Id = e.Id // for ordering
                                  });


        }

    }
}
