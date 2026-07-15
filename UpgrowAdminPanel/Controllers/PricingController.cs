using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
using Upgrow.Application.DTO.Pricing;
using Upgrow.Application.IServices;
using Upgrow.Application.IServices.Api;
using Upgrow.Application.IServices.Master;
using Upgrow.Application.IServices.Pricing;
using Upgrow.Application.Services;
using Upgrow.Application.Services.WL;

namespace UpgrowAdminPanel.Controllers
{
    public class PricingController : BaseController
    {
        // =====================================
        // SERVICES
        // =====================================

        private readonly IDomainResolverService
            _domainResolverService;

        private readonly IMasterApiCategoryService
            _apiCategoryService;

        private readonly IMasterPricingService
            _pricingService;

        private readonly IApiProviderService
            _apiProviderService;

        private readonly ICommonService
            _commonService;

        // =====================================
        // CONSTANTS
        // =====================================

        private const string NEW_PRICING =
            "newpricing";

        private const string CURRENT_PRICING =
            "currentpricing";

        // =====================================
        // CONSTRUCTOR
        // =====================================

        public PricingController(
            IEncryptionService encryptionService,
            IDataTableParser dataTableParser,
            IDomainResolverService domainResolverService,
            IApiProviderService apiProviderService,
            IMasterPricingService providerPricingservice,
            ICommonService commonService,
            IMasterApiCategoryService apiCategoryService
        ) : base(dataTableParser, domainResolverService, encryptionService)
        {
            _domainResolverService = domainResolverService;
            _pricingService = providerPricingservice;
            _apiCategoryService = apiCategoryService;
            _apiProviderService = apiProviderService;
            _commonService = commonService;

        }

        #region Manage Provider Pricing
        [HttpGet]
        public async Task<IActionResult> ManageProviderPricing()
        {
            var platformOwnerUUID = await _commonService.GetPlatformOwnerUUID();

            var vm = new PricingPageVM
            {
                PlatformOwnerUUID = platformOwnerUUID,

                PriceTypeList =
                    new List<SelectListItem>
                    {
                        new SelectListItem
                        {
                            Text = "New Price",
                            Value = NEW_PRICING
                        },

                        new SelectListItem
                        {
                            Text = "Active Price",
                            Value = CURRENT_PRICING
                        }
                    },

                CategoryList =
                    await LoadDropdownAsync(
                        _apiCategoryService,
                        x => x.CategoryName
                    ),
                ProviderList =
                    await LoadDropdownAsync(
                        _apiProviderService,
                        x => x.ProviderName
                        )
            };

            return View(vm);
        }

        // =====================================
        // GET PRICE TABLE
        // =====================================

        [HttpPost]
        public async Task<IActionResult> GetPriceTbl([FromBody] PricingFilterDto model)
        {
            if (!ModelState.IsValid)
            {
                return InvalidResponse(
                    "Invalid request."
                );
            }

            // =====================================
            // NULL CHECK
            // =====================================

            if (model == null)
            {
                return InvalidResponse(
                    "Invalid request."
                );
            }

            // =====================================
            // PRICE TYPE VALIDATION
            // =====================================

            var priceType =
                model.PriceType?
                    .Trim()
                    .ToLower();

            if (priceType != NEW_PRICING
                &&
                priceType != CURRENT_PRICING)
            {
                return InvalidResponse(
                    "Price type is required."
                );
            }

            // =====================================
            // PROVIDER VALIDATION
            // =====================================

            if (string.IsNullOrWhiteSpace(
                    model.ProviderUUID))
            {
                return InvalidResponse(
                    "Provider is required."
                );
            }

            // =====================================
            // DATE VALIDATION
            // =====================================

            if (string.IsNullOrWhiteSpace(
                    model.EffectiveDate))
            {
                return InvalidResponse(
                    "Effective date is required."
                );
            }

            // =====================================
            // DATE PARSE
            // =====================================

            bool isParsed =
                DateOnly.TryParseExact(
                    model.EffectiveDate,
                    "dd-MM-yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var effectiveDate
                );

            if (!isParsed)
            {
                return InvalidResponse(
                    "Invalid effective date format."
                );
            }

            // =====================================
            // SERVICE CALL
            // =====================================

            var result =
    await _pricingService
        .GetPriceTblAsync(
            priceType,
            model.ProviderUUID,
            effectiveDate
        );

            // =====================================
            // RESPONSE
            // =====================================

            return Json(new
            {
                success = result.success,

                message = result.message,

                pricing = result.data
            });
        }

        // =====================================
        // INVALID RESPONSE
        // =====================================

        private JsonResult InvalidResponse(
            string message)
        {
            return Json(new
            {
                success = false,

                message = message,

                pricing = (object)null
            });
        }

        // =====================================
        // SAVE PRICING
        // =====================================

        [HttpPost]
        public async Task<IActionResult> SavePricing([FromBody] SavePricingRequestDto model)
        {
            try
            {
                if (model == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Invalid request."
                    });
                }

                var result =
                    await _pricingService
                        .SavePricingAsync(model);

                return Json(new
                {
                    success = result.success,
                    message = result.message
                });
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Manage Customer Pricing
        [HttpGet]
        public async Task<IActionResult> ManageCustomerPricing()
        {
            var platformOwnerTask = _commonService.GetPlatformOwnerUUID();
            var categoryListTask = LoadDropdownAsync(_apiCategoryService, x => x.CategoryName);

            await Task.WhenAll(platformOwnerTask, categoryListTask);

            var vm = new PricingPageVM
            {
                PlatformOwnerUUID = await platformOwnerTask,

                PriceTypeList =
                    new List<SelectListItem>
                    {
                        new SelectListItem
                        {
                            Text = "New Price",
                            Value = NEW_PRICING
                        },

                        new SelectListItem
                        {
                            Text = "Active Price",
                            Value = CURRENT_PRICING
                        }
                    },

                CategoryList = await categoryListTask

            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> GetCustomerPriceTbl([FromBody] CWPricingFilterDto model)
        {
            if (!ModelState.IsValid || model == null)
            {
                return InvalidResponse(
                    "Invalid request."
                );
            }

            var priceType = model.PriceType?.Trim().ToLower();

            if (priceType != NEW_PRICING
                &&
                priceType != CURRENT_PRICING)
            {
                return InvalidResponse(
                    "Price type is required."
                );
            }

            if (string.IsNullOrWhiteSpace(model.EffectiveDate))
            {
                return InvalidResponse(
                    "Effective date is required."
                );
            }

            bool isParsed =
                DateOnly.TryParseExact(
                    model.EffectiveDate,
                    "dd-MM-yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var effectiveDate
                );

            if (!isParsed)
            {
                return InvalidResponse(
                    "Invalid effective date format."
                );
            }

            var platformOwnerUuid =
                await _commonService.GetPlatformOwnerUUID();

            var result =
                await _pricingService
                    .GetCustomerPriceTblAsync(
                        priceType,
                        platformOwnerUuid,
                        effectiveDate
                    );

            return Json(new
            {
                success = result.success,

                message = result.message,

                pricing = result.data
            });
        }

        [HttpPost]
        public async Task<IActionResult> SaveCustomerPricing([FromBody] SavePricingRequestDto model)
        {
            if (model == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Invalid request."
                });
            }

            var result =
                await _pricingService
                    .SaveCustomerPricingAsync(model);

            return Json(new
            {
                success = result.success,
                message = result.message
            });
        }
        #endregion
    }
}
