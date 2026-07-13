using Microsoft.AspNetCore.Mvc;
using Upgrow.Application.Commands.Credential;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.DTO.Credential;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.IServices;
using Upgrow.Application.IServices.Credential;
using Upgrow.Infrastructure.Filters;

namespace UpgrowAdminPanel.Controllers
{
    public class CredentialController : BaseController
    {
        private readonly IPaymentGatewayCredentialService _paymentgatewayService;
        public CredentialController(
            IPaymentGatewayCredentialService paymentgatewayService,
            IDataTableParser dataTableParser, 
            IDomainResolverService tenantDomainResolver, 
            IEncryptionService encryptionService) : base(dataTableParser, tenantDomainResolver, encryptionService)
        {
            _paymentgatewayService = paymentgatewayService;
        }

        #region payment gateway credential

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened PaymentGateway Credentials List", MenuName = "PaymentGatewayCredential")]
        public IActionResult ViewPaymentGatewayCredential() => View();

        [HttpPost]
        public Task<IActionResult> GetPaymentGatewayCredentials()
            => GetPagedDataAsync<PaymentGatewayCredentialDto, PaymentGatewayCredentialCommand>(_paymentgatewayService, dto => new Dictionary<string, object>
            {
                ["uuid"] = dto.UUID,
                ["name"] = dto.Name,
                ["code"] = dto.Code,
                ["apikey"] = dto.ApiKey,
                ["merchantId"] = dto.MerchantId,
                ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(TogglePaymentGatewayCredential), "PaymentGateway Credentials")
            });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master PaymentGateway Credentials", MenuName = "PaymentGatewayCredential")]
        public IActionResult AddPaymentGatewayCredential() => View("AddPaymentGatewayCredential", new PaymentGatewayCredentialCommand { IsActive = true });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Master PaymentGateway Credentials", MenuName = "PaymentGatewayCredential")]
        public Task<IActionResult> EditPaymentGatewayCredential(string? uuid)
            => EditMasterAsync(
                uuid,
                _paymentgatewayService,
                () => new PaymentGatewayCredentialCommand { IsActive = true },
                dto => new PaymentGatewayCredentialCommand
                {
                    UUID = dto.UUID,
                    Name = dto.Name,
                    MerchantId = dto.MerchantId,
                    Code = dto.Code,
                    ApiKey = dto.ApiKey,
                    EncryptedKeySecret = string.Empty,
                    IsActive = dto.IsActive
                },
                "AddPaymentGatewayCredential",
                nameof(ViewPaymentGatewayCredential));

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master PaymentGateway Credentials", MenuName = "PaymentGatewayCredential")]
        public Task<IActionResult> AddPaymentGatewayCredential(PaymentGatewayCredentialCommand command)
            => SaveMasterAsync(
                command,
                _paymentgatewayService,
                "PaymentGateway Credential",
                "AddPaymentGatewayCredential",
                nameof(ViewPaymentGatewayCredential));
        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Master PaymentGateway Credentials Status", MenuName = "PaymentGatewayCredential")]
        public Task<IActionResult> TogglePaymentGatewayCredential(string uuid)
            => ToggleActiveAsync<PaymentGatewayCredentialDto, PaymentGatewayCredentialCommand>(uuid, _paymentgatewayService, "PaymentGateway Credentials");

        #endregion
    }
}
