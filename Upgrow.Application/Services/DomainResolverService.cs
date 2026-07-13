// Upgrow.Application.Services.WL\TenantDomainResolverService.cs
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using Upgrow.Application.IServices;
using Upgrow.Application.IServices.WL;
using Upgrow.Application.Options;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.Services.WL
{
    public class DomainResolverService : IDomainResolverService
    {
        private readonly IMasterRepository<TenantDomain> _tenantDomainRepo;
        private readonly IMasterRepository<Tenant> _tenantRepo;
        //private readonly IConfigurationRepository _configurationRepository;
        private readonly string? _fileBaseUrl;

        public DomainResolverService(
            IMasterRepository<TenantDomain> tenantDomainRepo,
            IMasterRepository<Tenant> tenantRepo,
            IOptions<FileUploadOptions> fileUploadOptions)
        {
            //_configurationRepository = configurationRepository;            
            _tenantDomainRepo = tenantDomainRepo;
            _tenantRepo = tenantRepo;
            _fileBaseUrl = NormalizeBaseUrl(fileUploadOptions.Value.BaseUrl);

        }

        public async Task<string?> ResolveCompanyIdentifierAsync(string host)
        {
            if (string.IsNullOrWhiteSpace(host)) return null;

            host = host.Trim().ToLowerInvariant();
            if (host.StartsWith("www.", StringComparison.OrdinalIgnoreCase))
                host = host.Substring(4);

            // Get active tenant domains (small table — OK to fetch)
            var domains = await _tenantDomainRepo.GetAllActiveAsync();

            var match = domains.FirstOrDefault(td =>
                !string.IsNullOrWhiteSpace(td.Domain) &&
                (string.Equals(td.Domain.Trim().ToLowerInvariant(), host, StringComparison.OrdinalIgnoreCase)
                 || host.EndsWith("." + td.Domain.Trim().ToLowerInvariant(), StringComparison.OrdinalIgnoreCase)));

            if (match == null) return null;

            var tenants = await _tenantRepo.GetAllActiveAsync();
            var tenant = tenants.FirstOrDefault(t => t.Id == (decimal)match.TenantId);

            return tenant?.Identifier;
        }

        //public Task<string?> BuildAbsoluteUrl(string? path)
        //{
        //    if (string.IsNullOrWhiteSpace(path))
        //        return Task.FromResult<string?>(string.Empty);

        //    var value = path.Trim();

        //    if (Uri.TryCreate(value, UriKind.Absolute, out _))
        //        return Task.FromResult<string?>(value);

        //    value = value.TrimStart('~').TrimStart('/');

        //    if (string.IsNullOrWhiteSpace(_fileBaseUrl))
        //        return Task.FromResult<string?>("/" + value);

        //    return Task.FromResult<string?>($"{_fileBaseUrl}/{value}");
        //}

        public string BuildAbsoluteUrl(string? path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return string.Empty;

            var value = path.Trim();

            if (Uri.TryCreate(value, UriKind.Absolute, out _))
                return value;

            value = value.TrimStart('~').TrimStart('/');

            if (string.IsNullOrWhiteSpace(_fileBaseUrl))
                return "/" + value;

            return $"{_fileBaseUrl}/{value}";
        }

        private static string? NormalizeBaseUrl(string? baseUrl)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
                return null;

            baseUrl = baseUrl.Trim().TrimEnd('/');

            if (!baseUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                !baseUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                baseUrl = $"https://{baseUrl}";
            }

            return baseUrl;
        }
    }
}