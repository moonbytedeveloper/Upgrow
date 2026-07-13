using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.Pricing;
using VerifyIndia.Application.Interfaces.Pricing;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Infrastructure.Repositories.Pricing
{
    public class MasterPricingRepository
    : IMasterPricingRepository
    {
        private readonly AppDbContext _context;

        public MasterPricingRepository(
            AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<ApiPricingProjectionDto> GetPricingQuery(
            DateOnly effectiveDate,
            string providerUUID)
        {
            return

                from pa in _context.ProviderApis

                where pa.IsActive && pa.ProviderUUID == providerUUID

                join p in _context.Api_Provider
                    on pa.ProviderUUID equals p.UUID

                join a in _context.Master_Api
                    on pa.ApiUUID equals a.UUID

                join c in _context.Api_Category
                    on a.ApiCategoryUUID equals c.UUID
                    into catGroup

                from c in catGroup.DefaultIfEmpty()

                join mp in _context.Master_Pricing
                    .Where(x =>
                        x.IsActive &&
                        x.SellerTenantId == null &&
                        x.EffectiveFrom <= effectiveDate &&
                        (
                            x.EffectiveTo == null ||
                            x.EffectiveTo >= effectiveDate
                        ))
                    on new
                    {
                        ApiUUID = a.UUID,
                        ProviderUUID = p.UUID
                    }
                    equals new
                    {
                        ApiUUID = mp.APIUUID,
                        ProviderUUID = mp.ProviderUUID
                    }

                    into pricingGroup

                from mp in pricingGroup
                    .DefaultIfEmpty()
                where a.IsActive
                orderby
                    p.ProviderName,
                    a.ApiName

                select new ApiPricingProjectionDto
                {
                    APIUUID = a.UUID,
                    APIName = a.ApiName,
                    APICode = a.Code,
                    CategoryUUID = c != null ? c.UUID : "",
                    CategoryName = c != null ? c.CategoryName : "",
                    ProviderUUID = p.UUID,
                    ProviderName = p.ProviderName,
                    PricingUUID = mp != null ? mp.UUID : "",
                    CurrentPrice = mp != null ? mp.BaseAmount : null,
                    CurrentPriceMB = mp != null ? mp.BaseAmountMB : null,
                    EffectiveFrom = mp != null ? mp.EffectiveFrom : default,
                    EffectiveTo = mp != null ? mp.EffectiveTo : null
                };
        }

        public IQueryable<ApiPricingProjectionDto>
    GetCustomerPricingQuery(
        DateOnly effectiveDate,
        string platformOwnerUuid)
        {
            return

                from a in _context.Master_Api

                where a.IsActive

                join c in _context.Api_Category
                    on a.ApiCategoryUUID equals c.UUID
                    into catGroup

                from c in catGroup.DefaultIfEmpty()

                join mpCustomer in _context.Master_Pricing

                    .Where(x =>

                        x.IsActive

                        &&

                        x.SellerTenantId ==
                            platformOwnerUuid

                        &&

                        x.ProviderUUID == null

                        &&

                        x.BuyerType ==
                            "Customer"

                        &&

                        x.BuyerUUID == null

                        &&

                        x.EffectiveFrom <=
                            effectiveDate

                        &&

                        (
                            x.EffectiveTo == null

                            ||

                            x.EffectiveTo >=
                                effectiveDate
                        ))

                    on a.UUID equals mpCustomer.APIUUID

                    into customerGroup

                from mpCustomer in customerGroup
                    .DefaultIfEmpty()

                orderby
                    c.CategoryName,
                    a.ApiName

                select new ApiPricingProjectionDto
                {
                    APIUUID =
                        a.UUID,

                    APIName =
                        a.ApiName,

                    APICode =
                        a.Code,

                    CategoryUUID =
                        c != null
                            ? c.UUID
                            : "",

                    CategoryName =
                        c != null
                            ? c.CategoryName
                            : "",

                    ProviderUUID =
                        "",

                    ProviderName =
                        "",

                    PricingUUID =
                        mpCustomer != null
                            ? mpCustomer.UUID
                            : "",

                    CurrentPrice =
                        mpCustomer != null
                            ? mpCustomer.BaseAmount
                            : null,

                    CurrentPriceMB =
                        null,

                    PurchasePrice =
                        null,

                    EffectiveFrom =
                        mpCustomer != null
                            ? mpCustomer.EffectiveFrom
                            : default,

                    EffectiveTo =
                        mpCustomer != null
                            ? mpCustomer.EffectiveTo
                            : null
                };
        }

        public IQueryable<ApiPricingProjectionDto>
    GetWhiteLabelPricingQuery(
        DateOnly effectiveDate,
        string platformOwnerUuid)
        {
            return

                from a in _context.Master_Api

                where a.IsActive

                join c in _context.Api_Category
                    on a.ApiCategoryUUID equals c.UUID
                    into catGroup

                from c in catGroup.DefaultIfEmpty()

                join mpWhiteLabel in _context.Master_Pricing

                    .Where(x =>

                        x.IsActive

                        &&

                        x.SellerTenantId ==
                            platformOwnerUuid

                        &&

                        x.ProviderUUID == null

                        &&

                        x.BuyerType ==
                            "Tenant"

                        &&

                        x.BuyerUUID == null

                        &&

                        x.EffectiveFrom <=
                            effectiveDate

                        &&

                        (
                            x.EffectiveTo == null

                            ||

                            x.EffectiveTo >=
                                effectiveDate
                        ))

                    on a.UUID equals mpWhiteLabel.APIUUID

                    into whiteLabelGroup

                from mpWhiteLabel in whiteLabelGroup
                    .DefaultIfEmpty()

                orderby
                    c.CategoryName,
                    a.ApiName

                select new ApiPricingProjectionDto
                {
                    APIUUID =
                        a.UUID,

                    APIName =
                        a.ApiName,

                    APICode =
                        a.Code,

                    CategoryUUID =
                        c != null
                            ? c.UUID
                            : "",

                    CategoryName =
                        c != null
                            ? c.CategoryName
                            : "",

                    ProviderUUID =
                        "",

                    ProviderName =
                        "",

                    PricingUUID =
                        mpWhiteLabel != null
                            ? mpWhiteLabel.UUID
                            : "",

                    CurrentPrice =
                        mpWhiteLabel != null
                            ? mpWhiteLabel.BaseAmount
                            : null,

                    CurrentPriceMB =
                        null,

                    PurchasePrice =
                        null,

                    EffectiveFrom =
                        mpWhiteLabel != null
                            ? mpWhiteLabel.EffectiveFrom
                            : default,

                    EffectiveTo =
                        mpWhiteLabel != null
                            ? mpWhiteLabel.EffectiveTo
                            : null
                };
        }

        public async Task<IQueryable<Master_Pricing>> GetProviderExistingPricing(
            string providerUUID)
        {
            return _context.Master_Pricing
                .Where(x =>
                    x.IsActive &&
                    x.SellerTenantId == null &&
                    x.ProviderUUID == providerUUID
                );
        }

        public async Task<IQueryable<Master_Pricing>> GetCustomerExistingPricing(
            string sellerTenantId)
        {
            return _context.Master_Pricing
                .Where(x =>
                    x.IsActive &&
                    x.SellerTenantId == sellerTenantId &&
                    x.ProviderUUID == null &&
                    x.BuyerType == "Customer" &&
                    x.BuyerUUID == null
                );
        }

        public async Task<IQueryable<Master_Pricing>> GetWhiteLabelExistingPricing(
            string sellerTenantId)
        {
            return _context.Master_Pricing
                .Where(x =>
                    x.IsActive &&
                    x.SellerTenantId == sellerTenantId &&
                    x.ProviderUUID == null &&
                    x.BuyerType == "Tenant" &&
                    x.BuyerUUID == null
                );
        }

        public async Task AddAsync(
            Master_Pricing entity)
        {
            await _context.Master_Pricing
                .AddAsync(entity);
        }

        public void Update(
            Master_Pricing entity)
        {
            _context
                .Master_Pricing
                .Update(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context
                .SaveChangesAsync();
        }
    }
}
