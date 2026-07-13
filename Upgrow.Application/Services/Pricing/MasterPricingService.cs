using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.DTO.Pricing;
using Upgrow.Application.Interfaces.Pricing;
using Upgrow.Application.IServices.Pricing;
using Upgrow.Domain.Entities;

namespace Upgrow.Application.Services.Pricing
{
    public class MasterPricingService
        : IMasterPricingService
    {
        private readonly IMasterPricingRepository
            _repo;

        public MasterPricingService(
            IMasterPricingRepository repo)
        {
            _repo = repo;
        }

        public async Task<(bool success, string message, List<PricingGroupDto> data)> GetPriceTblAsync(
                string priceType,
                string providerUUID,
                DateOnly effectiveDate)
        {
            var pricingEntities = await _repo.GetPricingQuery(effectiveDate, providerUUID).ToListAsync();
                    
            // =====================================
            // NEW PRICING
            // =====================================

            if (priceType == "newpricing")
            {
                // =====================================
                // OVERLAPPING RECORDS
                // =====================================

                var overlapping = pricingEntities
                    .Where(x =>
                        (x.EffectiveTo != null &&
                            effectiveDate >= x.EffectiveFrom &&
                            effectiveDate <= x.EffectiveTo)
                        ||

                        (x.EffectiveTo == null &&
                        effectiveDate == x.EffectiveFrom
                    ))
                    .ToList();

                if (overlapping.Any())
                {
                    return (
                        false,
                        "A pricing record already exists for the selected date.",
                        null
                    );
                }

                // =====================================
                // RETURN EMPTY GROUPS
                // =====================================

                var pricingGroups = pricingEntities

                    .GroupBy(x => x.CategoryName)

                    .Select(g => new PricingGroupDto
                    {
                        CategoryName = g.Key,

                        APIs = g.ToList()
                    })

                    .OrderBy(x => x.CategoryName)

                    .ToList();

                return (
                    true,
                    "",
                    pricingGroups
                );
            }

            // =====================================
            // CURRENT PRICING
            // =====================================

            else if (priceType == "currentpricing")
            {
                var pricedEntities = pricingEntities
                    .Where(x => !string.IsNullOrEmpty(x.PricingUUID))
                    .ToList();

                if (!pricedEntities.Any())
                {
                    return (
                        false,
                        "Record does not exist for selected date.",
                        null
                    );
                }

                // =====================================
                // GROUP BY CATEGORY
                // =====================================

                var pricingGroups = pricingEntities

                    .GroupBy(x => x.CategoryName)

                    .Select(g => new PricingGroupDto
                    {
                        CategoryName = g.Key,

                        APIs = g.ToList()
                    })

                    .OrderBy(x => x.CategoryName)

                    .ToList();

                return (
                    true,
                    "",
                    pricingGroups
                );
            }

            // =====================================
            // INVALID TYPE
            // =====================================

            return (
                false,
                "Invalid request.",
                null
            );
        }

        public async Task<(bool success, string message)> SavePricingAsync(
            SavePricingRequestDto request)
        {

            // =====================================
            // VALIDATION
            // =====================================

            if (request == null
                || string.IsNullOrEmpty(request.PriceType)
                || string.IsNullOrWhiteSpace(request.ProviderUUID)
                || request.EffectiveDate == default
                || request.PricingList == null
                || !request.PricingList.Any())
            {
                return (
                    false,
                    "Missing required parameters."
                );
            }

            if (request.PricingList.Any(x => x.BaseAmount <= 0 || (x.SellerTenantId == null && x.BaseAmountMB.HasValue && x.BaseAmountMB <= 0)))
            {
                return (
                    false,
                    "All prices are required and must be greater than 0."
                );
            }

            // ============================================
            // FETCH EXISTING PRICING RELATED TO PROVIDERS
            // ============================================

            var allExisting = await _repo.GetProviderExistingPricing(request.ProviderUUID);


            var overLapping = allExisting.Where(x =>
                        x.EffectiveTo != null
                        ? (request.EffectiveDate >= x.EffectiveFrom && request.EffectiveDate <= x.EffectiveTo)
                        : (request.EffectiveDate >= x.EffectiveFrom)
            ).ToList();

            if (request.PriceType == "newpricing")
            {
                // Exact same EffectiveFrom Open/Close ended Records
                var exactOpenCloseEffectiveFrom = overLapping.Where(x =>
                       request.EffectiveDate == x.EffectiveFrom
                ).Any();

                if (exactOpenCloseEffectiveFrom)
                {
                    return (
                        false,
                        "Record already exists for the selected date; please update pricing."
                        );
                }

                // Update existing open ended or overlapping closed range
                var existingRecords = overLapping.Where(x =>
                        request.EffectiveDate > x.EffectiveFrom
                ).ToList();

                foreach (var rec in existingRecords)
                {
                    rec.EffectiveTo = request.EffectiveDate.AddDays(-1);
                    _repo.Update(rec);
                }

                // Insert the new pricing
                foreach (var input in request.PricingList)
                {
                    // Filter matching future records for current combination
                    var matchingFutureCombination = allExisting
                        .Where(x =>
                            x.EffectiveFrom > request.EffectiveDate &&
                            x.APIUUID == input.APIUUID &&
                            x.ProviderUUID == input.ProviderUUID &&
                            x.BuyerType == input.BuyerType &&
                            x.BuyerUUID == input.BuyerUUID
                        ).OrderBy(x => x.EffectiveFrom)
                        .FirstOrDefault();
                    ;

                    DateOnly? effectiveTo = null;
                    if (matchingFutureCombination != null)
                    {
                        effectiveTo = matchingFutureCombination.EffectiveFrom.AddDays(-1);
                    }

                    var newRec = new Master_Pricing
                    {
                        UUID = Utils.GetUUID(),
                        APIUUID = input.APIUUID,
                        SellerTenantId = null,
                        ProviderUUID = input.ProviderUUID,
                        BuyerType = "Tenant",
                        BuyerUUID = input.BuyerUUID,
                        BaseAmount = input.BaseAmount,
                        BaseAmountMB = input.BaseAmountMB,
                        EffectiveFrom = request.EffectiveDate,
                        EffectiveTo = effectiveTo,
                        IsActive = true
                    };

                    await _repo.AddAsync(newRec);
                }

                await _repo.SaveChangesAsync();

                return (
                    true,
                    "Pricing added successfully."
                    );
            }

            else if (request.PriceType == "currentpricing")
            {
                if (!overLapping.Any())
                {
                    return (
                        false,
                        "Record does not exist for selected date."
                    );
                }

                // Preserve original window before updates
                DateOnly? originalEffectiveTo =
                    overLapping
                        .Where(x =>
                            x.EffectiveTo != null
                        )
                        .Select(x =>
                            x.EffectiveTo
                        )
                        .FirstOrDefault();

                foreach (var input in request.PricingList)
                {
                    var match = overLapping.FirstOrDefault(x =>
                        x.EffectiveFrom == request.EffectiveDate &&
                        x.APIUUID == input.APIUUID &&
                        x.ProviderUUID == input.ProviderUUID &&
                        x.BuyerType == input.BuyerType &&
                        x.BuyerUUID == input.BuyerUUID
                    );

                    if (match != null)
                    {
                        match.BaseAmount = input.BaseAmount;
                        match.BaseAmountMB = input.BaseAmountMB;
                        _repo.Update(match);
                        continue;
                    }

                    // overlapping open ended record (EffectiveTo is null)
                    var openRec = overLapping.FirstOrDefault(x =>
                        x.EffectiveFrom < request.EffectiveDate &&
                        x.EffectiveTo == null &&
                        x.APIUUID == input.APIUUID &&
                        x.ProviderUUID == input.ProviderUUID &&
                        x.BuyerType == input.BuyerType &&
                        x.BuyerUUID == input.BuyerUUID
                    );

                    if (openRec != null)
                    {
                        openRec.EffectiveTo = request.EffectiveDate.AddDays(-1);
                        _repo.Update(openRec);
                    }

                    // overlapping close ended record (EffectiveTo >= newDate > EffectiveFrom)
                    var closedOverlap = overLapping.FirstOrDefault(x =>
                            x.EffectiveFrom < request.EffectiveDate &&
                            x.EffectiveTo != null &&
                            x.EffectiveTo >= request.EffectiveDate &&
                            x.APIUUID == input.APIUUID &&
                            x.ProviderUUID == input.ProviderUUID &&
                            x.BuyerType == input.BuyerType &&
                            x.BuyerUUID == input.BuyerUUID
                        );

                    DateOnly? clonedEffectiveTo = null;

                    if (closedOverlap != null)
                    {
                        clonedEffectiveTo =
                            closedOverlap.EffectiveTo;

                        closedOverlap.EffectiveTo =
                            request.EffectiveDate
                                .AddDays(-1);

                        _repo.Update(
                            closedOverlap
                        );
                    }

                    // New API case
                    else if (openRec == null)
                    {
                        clonedEffectiveTo = originalEffectiveTo;
                    }

                    // Insert new record
                    var newRec = new Master_Pricing
                    {
                        UUID = Utils.GetUUID(),
                        APIUUID = input.APIUUID,
                        SellerTenantId = null,
                        ProviderUUID = input.ProviderUUID,
                        BuyerType = "Tenant",
                        BuyerUUID = input.BuyerUUID,
                        BaseAmount = input.BaseAmount,
                        BaseAmountMB = input.BaseAmountMB,
                        EffectiveFrom = request.EffectiveDate,
                        EffectiveTo = clonedEffectiveTo, // from closed record if applicable otherwise null
                        IsActive = true
                    };

                    await _repo.AddAsync(newRec);
                }

                await _repo.SaveChangesAsync();

                return (
                    true,
                    "Pricing updated successfully."
                );
            }

            return (
                false,
                "Invalid request."
            );
        }

        public async Task<(bool success, string message, List<PricingGroupDto> data)> GetCustomerPriceTblAsync(
                    string priceType,
                    //string providerUUID,
                    string platformOwnerUuid,
                    DateOnly effectiveDate)
        {
            var pricingEntities = await _repo.GetCustomerPricingQuery(
                                    effectiveDate,
                                    platformOwnerUuid)
                                .ToListAsync();

            if (priceType == "newpricing")
            {
                var overlapping = pricingEntities
                    .Where(x =>
                        !string.IsNullOrEmpty(x.PricingUUID) &&
                        (
                            (x.EffectiveTo != null &&
                                effectiveDate >= x.EffectiveFrom &&
                                effectiveDate <= x.EffectiveTo)
                            ||
                            (x.EffectiveTo == null &&
                                effectiveDate == x.EffectiveFrom)
                        ))
                    .ToList();

                if (overlapping.Any())
                {
                    return (
                        false,
                        "A pricing record already exists for the selected date.",
                        null
                    );
                }

                var pricingGroups = pricingEntities
                    .GroupBy(x => x.CategoryName)
                    .Select(g => new PricingGroupDto
                    {
                        CategoryName = g.Key,
                        APIs = g.ToList()
                    })
                    .OrderBy(x => x.CategoryName)
                    .ToList();

                return (
                    true,
                    "",
                    pricingGroups
                );
            }
            else if (priceType == "currentpricing")
            {
                var pricedEntities = pricingEntities
                    .Where(x => !string.IsNullOrEmpty(x.PricingUUID))
                    .ToList();

                if (!pricedEntities.Any())
                {
                    return (
                        false,
                        "Record does not exist for selected date.",
                        null
                    );
                }

                var pricingGroups = pricingEntities
                    .GroupBy(x => x.CategoryName)
                    .Select(g => new PricingGroupDto
                    {
                        CategoryName = g.Key,
                        APIs = g.ToList()
                    })
                    .OrderBy(x => x.CategoryName)
                    .ToList();

                return (
                    true,
                    "",
                    pricingGroups
                );
            }

            return (
                false,
                "Invalid request.",
                null
            );
        }

        public async Task<(bool success, string message)> SaveCustomerPricingAsync(
                SavePricingRequestDto request)
        {
            if (request == null
                || string.IsNullOrEmpty(request.PriceType)
                || request.EffectiveDate == default
                || request.PricingList == null
                || !request.PricingList.Any())
            {
                return (
                    false,
                    "Missing required parameters."
                );
            }

            if (request.PricingList.Any(x => x.BaseAmount <= 0))
            {
                return (
                    false,
                    "All prices are required and must be greater than 0."
                );
            }

            var sellerTenantId = request.PricingList
                .Select(x => x.SellerTenantId)
                .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(sellerTenantId))
            {
                return (
                    false,
                    "Seller tenant is required."
                );
            }

            var allExisting = await _repo.GetCustomerExistingPricing(
                sellerTenantId);

            var overLapping = allExisting.Where(x =>
                        x.EffectiveTo != null
                        ? (request.EffectiveDate >= x.EffectiveFrom && request.EffectiveDate <= x.EffectiveTo)
                        : (request.EffectiveDate >= x.EffectiveFrom)
            ).ToList();

            if (request.PriceType == "newpricing")
            {
                var exactOpenCloseEffectiveFrom = overLapping.Where(x =>
                       request.EffectiveDate == x.EffectiveFrom
                ).Any();

                if (exactOpenCloseEffectiveFrom)
                {
                    return (
                        false,
                        "Record already exists for the selected date; please update pricing."
                        );
                }

                var existingRecords = overLapping.Where(x =>
                        request.EffectiveDate > x.EffectiveFrom
                ).ToList();

                foreach (var rec in existingRecords)
                {
                    rec.EffectiveTo = request.EffectiveDate.AddDays(-1);
                    _repo.Update(rec);
                }

                foreach (var input in request.PricingList)
                {
                    var matchingFutureCombination = allExisting
                        .Where(x =>
                            x.EffectiveFrom > request.EffectiveDate &&
                            x.APIUUID == input.APIUUID &&
                            x.SellerTenantId == sellerTenantId &&
                            x.ProviderUUID == null &&
                            x.BuyerType == "Customer" &&
                            x.BuyerUUID == null
                        ).OrderBy(x => x.EffectiveFrom)
                        .FirstOrDefault();

                    DateOnly? effectiveTo = null;
                    if (matchingFutureCombination != null)
                    {
                        effectiveTo = matchingFutureCombination.EffectiveFrom.AddDays(-1);
                    }

                    var newRec = new Master_Pricing
                    {
                        UUID = Utils.GetUUID(),
                        APIUUID = input.APIUUID,
                        SellerTenantId = sellerTenantId,
                        ProviderUUID = null,
                        BuyerType = "Customer",
                        BuyerUUID = null,
                        BaseAmount = input.BaseAmount,
                        BaseAmountMB = null,
                        EffectiveFrom = request.EffectiveDate,
                        EffectiveTo = effectiveTo,
                        IsActive = true
                    };

                    await _repo.AddAsync(newRec);
                }

                try
                {
                    await _repo.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                    throw;
                }

                return (
                    true,
                    "Pricing added successfully."
                    );
            }
            else if (request.PriceType == "currentpricing")
            {
                if (!overLapping.Any())
                {
                    return (
                        false,
                        "Record does not exist for selected date."
                    );
                }

                DateOnly? originalEffectiveTo =
                    overLapping
                        .Where(x =>
                            x.EffectiveTo != null
                        )
                        .Select(x =>
                            x.EffectiveTo
                        )
                        .FirstOrDefault();

                foreach (var input in request.PricingList)
                {
                    var match = overLapping.FirstOrDefault(x =>
                        x.EffectiveFrom == request.EffectiveDate &&
                        x.APIUUID == input.APIUUID &&
                        x.SellerTenantId == sellerTenantId &&
                        x.ProviderUUID == null &&
                        x.BuyerType == "Customer" &&
                        x.BuyerUUID == null
                    );

                    if (match != null)
                    {
                        match.BaseAmount = input.BaseAmount;
                        match.BaseAmountMB = null;
                        _repo.Update(match);
                        continue;
                    }

                    var openRec = overLapping.FirstOrDefault(x =>
                        x.EffectiveFrom < request.EffectiveDate &&
                        x.EffectiveTo == null &&
                        x.APIUUID == input.APIUUID &&
                        x.SellerTenantId == sellerTenantId &&
                        x.ProviderUUID == null &&
                        x.BuyerType == "Customer" &&
                        x.BuyerUUID == null
                    );

                    if (openRec != null)
                    {
                        openRec.EffectiveTo = request.EffectiveDate.AddDays(-1);
                        _repo.Update(openRec);
                    }

                    var closedOverlap = overLapping.FirstOrDefault(x =>
                            x.EffectiveFrom < request.EffectiveDate &&
                            x.EffectiveTo != null &&
                            x.EffectiveTo >= request.EffectiveDate &&
                            x.APIUUID == input.APIUUID &&
                            x.SellerTenantId == sellerTenantId &&
                            x.ProviderUUID == null &&
                            x.BuyerType == "Customer" &&
                            x.BuyerUUID == null
                        );

                    DateOnly? clonedEffectiveTo = null;

                    if (closedOverlap != null)
                    {
                        clonedEffectiveTo =
                            closedOverlap.EffectiveTo;

                        closedOverlap.EffectiveTo =
                            request.EffectiveDate
                                .AddDays(-1);

                        _repo.Update(
                            closedOverlap
                        );
                    }
                    else if (openRec == null)
                    {
                        clonedEffectiveTo = originalEffectiveTo;
                    }

                    var newRec = new Master_Pricing
                    {
                        UUID = Utils.GetUUID(),
                        APIUUID = input.APIUUID,
                        SellerTenantId = sellerTenantId,
                        ProviderUUID = null,
                        BuyerType = "Customer",
                        BuyerUUID = null,
                        BaseAmount = input.BaseAmount,
                        BaseAmountMB = null,
                        EffectiveFrom = request.EffectiveDate,
                        EffectiveTo = clonedEffectiveTo,
                        IsActive = true
                    };

                    await _repo.AddAsync(newRec);
                }

                try
                {
                    await _repo.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                    throw;
                }

                return (
                    true,
                    "Pricing updated successfully."
                );
            }

            return (
                false,
                "Invalid request."
            );
        }


        public async Task<(bool success, string message, List<PricingGroupDto> data)> GetWhiteLabelPriceTblAsync(
                string priceType,
                string platformOwnerUuid,
                DateOnly effectiveDate)
        {
            var pricingEntities = await _repo.GetWhiteLabelPricingQuery(
                                    effectiveDate,
                                    platformOwnerUuid)
                                .ToListAsync();

            if (priceType == "newpricing")
            {
                var overlapping = pricingEntities
                    .Where(x =>
                        !string.IsNullOrEmpty(x.PricingUUID) &&
                        (
                            (x.EffectiveTo != null &&
                                effectiveDate >= x.EffectiveFrom &&
                                effectiveDate <= x.EffectiveTo)
                            ||
                            (x.EffectiveTo == null &&
                                effectiveDate == x.EffectiveFrom)
                        ))
                    .ToList();

                if (overlapping.Any())
                {
                    return (
                        false,
                        "A pricing record already exists for the selected date.",
                        null
                    );
                }

                var pricingGroups = pricingEntities
                    .GroupBy(x => x.CategoryName)
                    .Select(g => new PricingGroupDto
                    {
                        CategoryName = g.Key,
                        APIs = g.ToList()
                    })
                    .OrderBy(x => x.CategoryName)
                    .ToList();

                return (
                    true,
                    "",
                    pricingGroups
                );
            }
            else if (priceType == "currentpricing")
            {
                var pricedEntities = pricingEntities
                    .Where(x => !string.IsNullOrEmpty(x.PricingUUID))
                    .ToList();

                if (!pricedEntities.Any())
                {
                    return (
                        false,
                        "Record does not exist for selected date.",
                        null
                    );
                }

                var pricingGroups = pricingEntities
                    .GroupBy(x => x.CategoryName)
                    .Select(g => new PricingGroupDto
                    {
                        CategoryName = g.Key,
                        APIs = g.ToList()
                    })
                    .OrderBy(x => x.CategoryName)
                    .ToList();

                return (
                    true,
                    "",
                    pricingGroups
                );
            }

            return (
                false,
                "Invalid request.",
                null
            );
        }

        public async Task<(bool success, string message)> SaveWhiteLabelPricingAsync(
                SavePricingRequestDto request)
        {
            if (request == null
                || string.IsNullOrEmpty(request.PriceType)
                || request.EffectiveDate == default
                || request.PricingList == null
                || !request.PricingList.Any())
            {
                return (
                    false,
                    "Missing required parameters."
                );
            }

            if (request.PricingList.Any(x => x.BaseAmount <= 0))
            {
                return (
                    false,
                    "All prices are required and must be greater than 0."
                );
            }

            var sellerTenantId = request.PricingList
                .Select(x => x.SellerTenantId)
                .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(sellerTenantId))
            {
                return (
                    false,
                    "Seller tenant is required."
                );
            }

            var allExisting = await _repo.GetWhiteLabelExistingPricing(
                sellerTenantId);

            var overLapping = allExisting.Where(x =>
                        x.EffectiveTo != null
                        ? (request.EffectiveDate >= x.EffectiveFrom && request.EffectiveDate <= x.EffectiveTo)
                        : (request.EffectiveDate >= x.EffectiveFrom)
            ).ToList();

            if (request.PriceType == "newpricing")
            {
                var exactOpenCloseEffectiveFrom = overLapping.Where(x =>
                       request.EffectiveDate == x.EffectiveFrom
                ).Any();

                if (exactOpenCloseEffectiveFrom)
                {
                    return (
                        false,
                        "Record already exists for the selected date; please update pricing."
                        );
                }

                var existingRecords = overLapping.Where(x =>
                        request.EffectiveDate > x.EffectiveFrom
                ).ToList();

                foreach (var rec in existingRecords)
                {
                    rec.EffectiveTo = request.EffectiveDate.AddDays(-1);
                    _repo.Update(rec);
                }

                foreach (var input in request.PricingList)
                {
                    var matchingFutureCombination = allExisting
                        .Where(x =>
                            x.EffectiveFrom > request.EffectiveDate &&
                            x.APIUUID == input.APIUUID &&
                            x.SellerTenantId == sellerTenantId &&
                            x.ProviderUUID == null &&
                            x.BuyerType == "Tenant" &&
                            x.BuyerUUID == null
                        ).OrderBy(x => x.EffectiveFrom)
                        .FirstOrDefault();

                    DateOnly? effectiveTo = null;
                    if (matchingFutureCombination != null)
                    {
                        effectiveTo = matchingFutureCombination.EffectiveFrom.AddDays(-1);
                    }

                    var newRec = new Master_Pricing
                    {
                        UUID = Utils.GetUUID(),
                        APIUUID = input.APIUUID,
                        SellerTenantId = sellerTenantId,
                        ProviderUUID = null,
                        BuyerType = "Tenant",
                        BuyerUUID = null,
                        BaseAmount = input.BaseAmount,
                        BaseAmountMB = null,
                        EffectiveFrom = request.EffectiveDate,
                        EffectiveTo = effectiveTo,
                        IsActive = true
                    };

                    await _repo.AddAsync(newRec);
                }

                try
                {
                    await _repo.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                    throw;
                }

                return (
                    true,
                    "Pricing added successfully."
                    );
            }
            else if (request.PriceType == "currentpricing")
            {
                if (!overLapping.Any())
                {
                    return (
                        false,
                        "Record does not exist for selected date."
                    );
                }

                DateOnly? originalEffectiveTo =
                    overLapping
                        .Where(x =>
                            x.EffectiveTo != null
                        )
                        .Select(x =>
                            x.EffectiveTo
                        )
                        .FirstOrDefault();

                foreach (var input in request.PricingList)
                {
                    var match = overLapping.FirstOrDefault(x =>
                        x.EffectiveFrom == request.EffectiveDate &&
                        x.APIUUID == input.APIUUID &&
                        x.SellerTenantId == sellerTenantId &&
                        x.ProviderUUID == null &&
                        x.BuyerType == "Tenant" &&
                        x.BuyerUUID == null
                    );

                    if (match != null)
                    {
                        match.BaseAmount = input.BaseAmount;
                        match.BaseAmountMB = null;
                        _repo.Update(match);
                        continue;
                    }

                    var openRec = overLapping.FirstOrDefault(x =>
                        x.EffectiveFrom < request.EffectiveDate &&
                        x.EffectiveTo == null &&
                        x.APIUUID == input.APIUUID &&
                        x.SellerTenantId == sellerTenantId &&
                        x.ProviderUUID == null &&
                        x.BuyerType == "Tenant" &&
                        x.BuyerUUID == null
                    );

                    if (openRec != null)
                    {
                        openRec.EffectiveTo = request.EffectiveDate.AddDays(-1);
                        _repo.Update(openRec);
                    }

                    var closedOverlap = overLapping.FirstOrDefault(x =>
                            x.EffectiveFrom < request.EffectiveDate &&
                            x.EffectiveTo != null &&
                            x.EffectiveTo >= request.EffectiveDate &&
                            x.APIUUID == input.APIUUID &&
                            x.SellerTenantId == sellerTenantId &&
                            x.ProviderUUID == null &&
                            x.BuyerType == "Tenant" &&
                            x.BuyerUUID == null
                        );

                    DateOnly? clonedEffectiveTo = null;

                    if (closedOverlap != null)
                    {
                        clonedEffectiveTo =
                            closedOverlap.EffectiveTo;

                        closedOverlap.EffectiveTo =
                            request.EffectiveDate
                                .AddDays(-1);

                        _repo.Update(
                            closedOverlap
                        );
                    }
                    else if (openRec == null)
                    {
                        clonedEffectiveTo = originalEffectiveTo;
                    }

                    var newRec = new Master_Pricing
                    {
                        UUID = Utils.GetUUID(),
                        APIUUID = input.APIUUID,
                        SellerTenantId = sellerTenantId,
                        ProviderUUID = null,
                        BuyerType = "Tenant",
                        BuyerUUID = null,
                        BaseAmount = input.BaseAmount,
                        BaseAmountMB = null,
                        EffectiveFrom = request.EffectiveDate,
                        EffectiveTo = clonedEffectiveTo,
                        IsActive = true
                    };

                    await _repo.AddAsync(newRec);
                }

                try
                {
                    await _repo.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                    throw;
                }

                return (
                    true,
                    "Pricing updated successfully."
                );
            }

            return (
                false,
                "Invalid request."
            );
        }

    }
}
