using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Upgrow.Application.Helper
{
    public sealed class CentralApiClient
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;

        public CentralApiClient(
            HttpClient client,
            IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
        }

        public async Task<JsonElement> PostAsync(
            string endpoint,
            object body,
            CancellationToken ct = default)
        {
            try
            {
                var centralApiBaseUrl = _configuration["CentralApi:BaseUrl"]
                    ?? throw new InvalidOperationException("CentralApi:BaseUrl is not configured");

                var requestUri = $"{centralApiBaseUrl.TrimEnd('/')}/api/verification/{endpoint}";

                using var request = new HttpRequestMessage(HttpMethod.Post, requestUri)
                {
                    Content = JsonContent.Create(body)
                };

                // Add authorization header if configured
                var authToken = _configuration["CentralApi:AuthToken"];
                if (!string.IsNullOrWhiteSpace(authToken))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
                }

                request.Headers.Add("Accept", "application/json");

                using var response = await _client.SendAsync(
                    request,
                    HttpCompletionOption.ResponseHeadersRead,
                    ct);

                var responseBody = await response.Content.ReadAsStringAsync(ct);

                if (!response.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException(
                        $"Central API endpoint '{endpoint}' returned {(int)response.StatusCode} " +
                        $"({response.StatusCode}). Response: {responseBody}");
                }
                //response.EnsureSuccessStatusCode();
                await using var stream = await response.Content.ReadAsStreamAsync(ct);
                using var doc = await JsonDocument.ParseAsync(stream, cancellationToken: ct);

                return doc.RootElement.Clone();
            }
            catch (HttpRequestException ex)
            {
                throw new InvalidOperationException($"Error calling Central API endpoint '{endpoint}': {ex.Message}", ex);
            }
        }
        public async Task<JsonElement> PostAsync(
            string endpoint,     
            CancellationToken ct = default)
        {
            try
            {
                object body = new { };
                var centralApiBaseUrl = _configuration["CentralApi:BaseUrl"]
                    ?? throw new InvalidOperationException("CentralApi:BaseUrl is not configured");

                var requestUri = $"{centralApiBaseUrl.TrimEnd('/')}/api/verification/{endpoint}";

                using var request = new HttpRequestMessage(HttpMethod.Post, requestUri)
                {
                    Content = JsonContent.Create(body)
                };

                // Add authorization header if configured
                var authToken = _configuration["CentralApi:AuthToken"];
                if (!string.IsNullOrWhiteSpace(authToken))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
                }

                request.Headers.Add("Accept", "application/json");

                using var response = await _client.SendAsync(
                    request,
                    HttpCompletionOption.ResponseHeadersRead,
                    ct);

                response.EnsureSuccessStatusCode();

                await using var stream = await response.Content.ReadAsStreamAsync(ct);
                using var doc = await JsonDocument.ParseAsync(stream, cancellationToken: ct);

                return doc.RootElement.Clone();
            }
            catch (HttpRequestException ex)
            {
                throw new InvalidOperationException($"Error calling Central API endpoint '{endpoint}': {ex.Message}", ex);
            }
        }
        public async Task<JsonElement> GetAsync(
            string endpoint,
            CancellationToken ct = default)
        {
            try
            {
                var centralApiBaseUrl = _configuration["CentralApi:BaseUrl"]
                    ?? throw new InvalidOperationException("CentralApi:BaseUrl is not configured");

                var requestUri = $"{centralApiBaseUrl.TrimEnd('/')}/api/verification/{endpoint}";

                using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

                // Add authorization header if configured
                var authToken = _configuration["CentralApi:AuthToken"];
                if (!string.IsNullOrWhiteSpace(authToken))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
                }

                request.Headers.Add("Accept", "application/json");

                using var response = await _client.SendAsync(
                    request,
                    HttpCompletionOption.ResponseHeadersRead,
                    ct);

                response.EnsureSuccessStatusCode();

                await using var stream = await response.Content.ReadAsStreamAsync(ct);
                using var doc = await JsonDocument.ParseAsync(stream, cancellationToken: ct);

                return doc.RootElement.Clone();
            }
            catch (HttpRequestException ex)
            {
                throw new InvalidOperationException($"Error calling Central API endpoint '{endpoint}': {ex.Message}", ex);
            }
        }
        // New: Post multipart/form-data requests (supports IFormFile and simple form fields)
        public async Task<JsonElement> PostMultipartAsync(
            string endpoint,
            object formModel,
            CancellationToken ct = default)
        {
            try
            {
                var centralApiBaseUrl = _configuration["CentralApi:BaseUrl"]
                    ?? throw new InvalidOperationException("CentralApi:BaseUrl is not configured");

                var requestUri = $"{centralApiBaseUrl.TrimEnd('/')}/api/verification/{endpoint}";

                using var multipart = new MultipartFormDataContent();

                if (formModel != null)
                {
                    var props = formModel.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    foreach (var p in props)
                    {
                        var value = p.GetValue(formModel);
                        if (value == null)
                            continue;

                        // Handle IFormFile
                        if (value is IFormFile file)
                        {
                            var streamContent = new StreamContent(file.OpenReadStream());
                            streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType ?? "application/octet-stream");
                            // name must match form field name expected by central API
                            multipart.Add(streamContent, p.Name, file.FileName);
                        }
                        // Handle collection of IFormFile
                        else if (value is IEnumerable<IFormFile> files)
                        {
                            foreach (var f in files)
                            {
                                var streamContent = new StreamContent(f.OpenReadStream());
                                streamContent.Headers.ContentType = new MediaTypeHeaderValue(f.ContentType ?? "application/octet-stream");
                                multipart.Add(streamContent, p.Name, f.FileName);
                            }
                        }
                        else
                        {
                            // For non-file fields, add as string content
                            var stringValue = value switch
                            {
                                DateTime dt => dt.ToString("o"),
                                _ => value.ToString()
                            } ?? string.Empty;

                            multipart.Add(new StringContent(stringValue), p.Name);
                        }
                    }
                }

                using var request = new HttpRequestMessage(HttpMethod.Post, requestUri)
                {
                    Content = multipart
                };

                // Add authorization header if configured
                var authToken = _configuration["CentralApi:AuthToken"];
                if (!string.IsNullOrWhiteSpace(authToken))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
                }

                request.Headers.Add("Accept", "application/json");

                using var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct);

                response.EnsureSuccessStatusCode();

                await using var stream = await response.Content.ReadAsStreamAsync(ct);
                using var doc = await JsonDocument.ParseAsync(stream, cancellationToken: ct);

                return doc.RootElement.Clone();
            }
            catch (HttpRequestException ex)
            {
                throw new InvalidOperationException($"Error calling Central API endpoint '{endpoint}': {ex.Message}", ex);
            }
        }

        public async Task<ApiResponse<T>> PostAsync<T>(
    string endpoint,
    object? body = null,
    CancellationToken ct = default)
        {
            try
            {
                var centralApiBaseUrl = _configuration["CentralApi:BaseUrl"]
                    ?? throw new InvalidOperationException("CentralApi:BaseUrl is not configured");

                var requestUri = $"{centralApiBaseUrl.TrimEnd('/')}/api/verification/{endpoint}";

                using var request = new HttpRequestMessage(HttpMethod.Post, requestUri)
                {
                    Content = body is null
                        ? JsonContent.Create(new { })
                        : JsonContent.Create(body)
                };

                var authToken = _configuration["CentralApi:AuthToken"];
                if (!string.IsNullOrWhiteSpace(authToken))
                {
                    request.Headers.Authorization =
                        new AuthenticationHeaderValue("Bearer", authToken);
                }

                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using var response = await _client.SendAsync(
                    request,
                    HttpCompletionOption.ResponseHeadersRead,
                    ct);

                var httpStatusCode = response.StatusCode;
                var result = await response.Content.ReadAsStringAsync(ct);

                if (string.IsNullOrWhiteSpace(result))
                {
                    return ApiResponse<T>.Fail(
                        response.ReasonPhrase ?? "Empty response received from Central API.",
                        httpStatusCode);
                }

                var trimmed = result.TrimStart();
                if (!trimmed.StartsWith("{", StringComparison.Ordinal) &&
                    !trimmed.StartsWith("[", StringComparison.Ordinal))
                {
                    return ApiResponse<T>.Fail(
                        $"Central API endpoint '{endpoint}' returned a non-JSON response: {Truncate(result)}",
                        httpStatusCode);
                }

                ApiResponse<T>? payload;
                try
                {
                    payload = JsonSerializer.Deserialize<ApiResponse<T>>(result, JsonOptions);
                }
                catch (JsonException ex)
                {
                    return ApiResponse<T>.Fail(
                        $"Error parsing Central API endpoint '{endpoint}': {ex.Message}. Raw response: {Truncate(result)}",
                        httpStatusCode);
                }

                if (payload is null)
                {
                    return ApiResponse<T>.Fail(
                        $"Unable to parse Central API response. Raw response: {Truncate(result)}",
                        httpStatusCode);
                }

                if (payload.StatusCode == default)
                {
                    payload.StatusCode = httpStatusCode;
                }

                return payload;
            }
            catch (HttpRequestException ex)
            {
                throw new InvalidOperationException(
                    $"Error calling Central API endpoint '{endpoint}': {ex.Message}", ex);
            }
        }

        private static string Truncate(string value, int maxLength = 500)
        {
            if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
            {
                return value;
            }

            return value[..maxLength] + "...";
        }
    }
}
