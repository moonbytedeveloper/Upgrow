using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.WL;
using VerifyIndia.Application.IServices;
using VerifyIndia.Application.IServices.WL.Master;
using VerifyIndia.Application.Utilities;
using VerifyIndia.Domain.Entities.WL.Master;
using VerifyIndia.Domain.IRepositories;
using VerifyIndia.Domain.IRepositories.WL;

namespace VerifyIndia.Application.Services.WL.Master
{
    public class WLActivityLogsLogsService : IWLActivityLogsLogsService
    {
        private readonly IWLActivityLogsRepository _repository;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;
        private readonly ITenantSetupService _tenantSetupService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WLActivityLogsLogsService(
            IWLActivityLogsRepository repository,
            IConfiguration config,
            IWebHostEnvironment env,
            ITenantSetupService tenantSetupService,
            IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _config = config;
            _env = env;
            _tenantSetupService = tenantSetupService;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Add activity log with hash chain and digital signature
        /// Flow:
        /// 1. Get last CurrentHash -> becomes PreviousHash for new entry
        /// 2. Serialize payload (JSON of activity data)
        /// 3. Compute CurrentHash (SHA256 of payload)
        /// 4. Compute DigitalSignature (RSA signature of CurrentHash)
        /// 5. Save to database
        /// </summary>
        public async Task AddAsync(WL_ActivityLogs log, WLActivityLogDto dto)
        {
            var tenantId = _tenantSetupService.TenantId;
            // Step 1: Serialize DTO to JSON payload with PascalCase property names
            var jsonOptions = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never,
                PropertyNamingPolicy = null  // Preserve PascalCase
            };
            var payload = JsonSerializer.Serialize(dto, jsonOptions);

            // Step 2: Get the previous log's current hash (for chain integrity) - filtered by tenant
            var previousHash = await _repository.GetLastCurrentHashAsync();

            // Step 3: Compute current hash from payload
            var currentHash = WLActivityLogHashUtility.ComputeCurrentHash(payload);

            // Step 4: Get RSA private key path            
            var privateKeyPath = KeyPathResolver.GetPrivateKeyPath();

            // Step 5: Compute digital signature
            var digitalSignature = WLActivityLogHashUtility.ComputeDigitalSignature(currentHash, privateKeyPath);

            // Step 6: Assign all values to entity
            log.CurrentHash = currentHash;
            log.PreviousHash = previousHash;
            log.DigitalSignature = digitalSignature;
            log.PayLoad = payload;
            log.TenantId = _tenantSetupService.TenantId;
            log.IsActive = true;

            // Step 7: Save to repository
            await _repository.AddAsync(log);
        }
    }
}