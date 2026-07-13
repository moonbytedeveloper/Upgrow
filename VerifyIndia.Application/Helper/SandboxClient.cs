using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Helper
{
    public sealed class SandboxClient
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private readonly IConfigurationRepository _configurationRepository;

        public SandboxClient(
            HttpClient client,
            IConfiguration configuration,
            IConfigurationRepository configurationRepository)
        {
            _client = client;
            _configuration = configuration;
            _configurationRepository = configurationRepository;
        }

        private async Task<Dictionary<string, string>> GetHeadersAsync()
        {
            var apiKey = _configuration["Sandbox:ApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new InvalidOperationException("Sandbox:ApiKey is not configured.");

            var accessToken = await _configurationRepository.GetSandboxAccessTokenAsync();
            if (string.IsNullOrWhiteSpace(accessToken))
                throw new InvalidOperationException("SandboxAccessToken is not available in Configuration table.");

            return new Dictionary<string, string>
            {
                { "x-api-key", apiKey },
                { "Authorization", accessToken },
                { "accept", "application/json" }
            };
        }

        public async Task<JsonElement> PostAsync(
           string endpoint,
           object body,
           string? entity,
           HttpMethod methodtype,
           CancellationToken ct = default)
        {
            object payload = body;

            if (!string.IsNullOrWhiteSpace(entity))
            {
                var jsonNode = JsonSerializer.SerializeToNode(body) as JsonObject ?? new JsonObject();
                jsonNode["@entity"] = entity;
                payload = jsonNode;
            }

            using var request = new HttpRequestMessage(methodtype, endpoint)
            {
                Content = JsonContent.Create(payload)
            };

            var headers = await GetHeadersAsync();

            foreach (var h in headers)
            {
                request.Headers.TryAddWithoutValidation(h.Key, h.Value);
            }

            using var response = await _client.SendAsync(
                request,
                HttpCompletionOption.ResponseHeadersRead,
                ct);

            response.EnsureSuccessStatusCode();

            await using var stream = await response.Content.ReadAsStreamAsync(ct);
            using var doc = await JsonDocument.ParseAsync(stream, cancellationToken: ct);

            return doc.RootElement.Clone();
        }

        public async Task<JsonElement> GetAsync(
            string endpoint,
            CancellationToken ct = default)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, endpoint);

            var headers = await GetHeadersAsync();
            foreach (var h in headers)
            {
                request.Headers.TryAddWithoutValidation(h.Key, h.Value);
            }

            using var response = await _client.SendAsync(
                request,
                HttpCompletionOption.ResponseHeadersRead,
                ct);

            response.EnsureSuccessStatusCode();

            await using var stream = await response.Content.ReadAsStreamAsync(ct);
            using var doc = await JsonDocument.ParseAsync(stream, cancellationToken: ct);

            return doc.RootElement.Clone();
        }
    }
}
