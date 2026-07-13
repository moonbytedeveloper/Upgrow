using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Json;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Upgrow.Application.Helper
{
    public sealed class SprintVerifyClient
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _config;
        public SprintVerifyClient(
            IConfiguration config,
            HttpClient client 
            )
        {
            _config = config;
            _client = client;
        }

        public string GenerateToken()
        {
            string key = _config["Jwt:SecretKey"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(credentials);


            var payload = new JwtPayload
        {
        { "timestamp", DateTimeOffset.Now.ToUnixTimeMilliseconds() },
        { "partnerId", _config["Jwt:PartnerId"] },
        { "reqid", new Random().Next(10000000,99999999).ToString() }
        };

            var token = new JwtSecurityToken(header, payload);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private Dictionary<string, string> GetHeaders()
        {
            return new Dictionary<string, string>
            {
                { "Token", GenerateToken() },
                //{ "authorisedkey", _config["Paysprint:AuthorisedKey"] },
                { "accept", "application/json" }
            };
        }

        public async Task<JsonElement> PostAsync(
    string endpoint,
    object? body = null,
    CancellationToken ct = default)
        {
            // Auto-generate refid ONLY if body exists and property exists
            if (body != null)
            {
                var prop = body.GetType().GetProperty("refid");

                if (prop != null && prop.PropertyType == typeof(string))
                {
                    var value = prop.GetValue(body)?.ToString();

                    if (string.IsNullOrWhiteSpace(value))
                    {
                        prop.SetValue(body, Utils.GetUUID());
                    }
                }
            }

            using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);

            // Add content only when body is provided
            if (body != null)
            {
                request.Content = JsonContent.Create(body);
            }

            var headers = GetHeaders();

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

            using var doc = await JsonDocument.ParseAsync(
                stream,
                cancellationToken: ct);

            return doc.RootElement.Clone();
        }
    }
}
