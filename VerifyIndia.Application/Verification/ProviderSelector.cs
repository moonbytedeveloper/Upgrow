using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Verification.Interfaces;
using VerifyIndia.Application.Verification.Verification;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Verification;

public class ProviderSelector : IProviderSelector
{
    private const int MaxProviderFallbackCount = 3;

    private readonly IEnumerable<IProviderAdapter> _providers;
    private readonly IMasterRepository<Master_Api> _masterApiRepository;
    private readonly IMasterRepository<Api_ProviderMapping> _apiProviderMappingRepository;
    private readonly IMasterRepository<Api_Provider> _apiProviderRepository;
    private readonly IMasterRepository<Api_Endpoint> _apiEndpointRepository;

    public ProviderSelector(
        IEnumerable<IProviderAdapter> providers,
        IMasterRepository<Master_Api> masterApiRepository,
        IMasterRepository<Api_ProviderMapping> apiProviderMappingRepository,
        IMasterRepository<Api_Provider> apiProviderRepository,
        IMasterRepository<Api_Endpoint> apiEndpointRepository)
    {
        _providers = providers;
        _masterApiRepository = masterApiRepository;
        _apiProviderMappingRepository = apiProviderMappingRepository;
        _apiProviderRepository = apiProviderRepository;
        _apiEndpointRepository = apiEndpointRepository;
    }

    public async Task<IReadOnlyList<IProviderAdapter>> SelectAsync(string verificationCode, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(verificationCode))
        {
            throw new ArgumentException("Verification code is required.", nameof(verificationCode));
        }

        var endpoint = (await _apiEndpointRepository.FindAllAsync(
        x => x.IsActive &&
             x.VerificationCode == verificationCode))
        .OrderBy(x => x.Sequence)
        .FirstOrDefault();

        if (endpoint is null)
        {
            throw new NotSupportedException(
                $"No active endpoint found for verification code '{verificationCode}'.");
        }

        if (string.IsNullOrWhiteSpace(endpoint.ApiUUID))
        {
            throw new NotSupportedException(
                $"ApiUUID not configured for verification code '{verificationCode}'.");
        }

        var api = (await _masterApiRepository.FindAllAsync(
            x => x.IsActive &&
                 x.UUID == endpoint.ApiUUID))
            .FirstOrDefault();

        if (api is null)
        {
            throw new NotSupportedException(
                $"No active API found for verification code '{verificationCode}'.");
        }

        var mappings = await _apiProviderMappingRepository.FindAllAsync(
            x => x.IsActive && x.ApiUUID == api.UUID);

        if (mappings.Count == 0)
        {
            throw new NotSupportedException(
                $"No active provider mapping found for verification code '{verificationCode}'.");
        }

        var orderedMappings = mappings
            .OrderBy(m => m.Priority ?? decimal.MaxValue)
            .ToList();

        var providerUuids = orderedMappings
            .Select(m => m.ProviderUUID)
            .Where(uuid => !string.IsNullOrWhiteSpace(uuid))
            .Distinct()
            .ToList();

        var providers = providerUuids.Count == 0
            ? new List<Api_Provider>()
            : await _apiProviderRepository.FindAllAsync(
                x => x.IsActive && providerUuids.Contains(x.UUID));

        var adapterLookup = _providers.ToDictionary(
            p => p.ProviderCode,
            StringComparer.OrdinalIgnoreCase);

        var selectedProviders = new List<IProviderAdapter>();
        var addedProviderCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var mapping in orderedMappings)
        {
            var providerUuid = mapping.ProviderUUID;
            if (string.IsNullOrWhiteSpace(providerUuid))
            {
                continue;
            }

            var provider = providers.FirstOrDefault(p => p.UUID == providerUuid);
            var providerCode = provider?.Code;

            if (string.IsNullOrWhiteSpace(providerCode))
            {
                continue;
            }

            if (!adapterLookup.TryGetValue(providerCode, out var adapter))
            {
                continue;
            }

            if (addedProviderCodes.Add(providerCode))
            {
                selectedProviders.Add(adapter);
            }
        }

        if (selectedProviders.Count == 0)
        {
            throw new NotSupportedException(
                $"No provider adapters available for verification code '{verificationCode}'.");
        }

        var maxProviders = api.IsProviderSwitchable == true ? MaxProviderFallbackCount : 1;

        return selectedProviders.Take(maxProviders).ToList();
    }
}
