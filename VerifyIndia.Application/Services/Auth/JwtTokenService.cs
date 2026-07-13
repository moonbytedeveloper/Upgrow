using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using VerifyIndia.Application.DTO.Auth;
using VerifyIndia.Application.DTO.Customer;
using VerifyIndia.Application.IServices.Auth;
using VerifyIndia.Application.Utilities;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.Entities.Auth;
using VerifyIndia.Domain.IRepositories;
using VerifyIndia.Domain.IRepositories.Master;

namespace VerifyIndia.Application.Services.Auth
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IMasterCustomerRepository _masterCustomerRepository;
        private readonly ITenantRepository _tenantRepository;

        public JwtTokenService(IConfiguration configuration, IRefreshTokenRepository refreshTokenRepository, IMasterCustomerRepository masterCustomerRepository,ITenantRepository tenantRepository)
        {
            _configuration = configuration;
            _refreshTokenRepository = refreshTokenRepository;
            _masterCustomerRepository = masterCustomerRepository;
            _tenantRepository = tenantRepository;
        }

        public async Task<TokenResponseDto> GenerateCustomerTokenAsync(Master_Customer customer)
        {
            var privateKeyPath = KeyPathResolver.GetPrivateKeyPath();

            if (!File.Exists(privateKeyPath))
                throw new InvalidOperationException($"JWT private key file not found: {privateKeyPath}");

            var privateKeyPem = File.ReadAllText(privateKeyPath);

            using var rsa = RSA.Create();
            rsa.ImportFromPem(privateKeyPem);

            var signingKey = new RsaSecurityKey(rsa.ExportParameters(true));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.RsaSha256);

            var expiryHours = int.TryParse(_configuration["Jwt:AccessTokenExpiryHours"], out var hours) && hours > 0
                ? hours
                : 24;

            var now = DateTime.UtcNow;
            var expiresAt = now.AddHours(expiryHours);

            var claims = new[]
            {
        new Claim("customerUUID", customer.UUID.ToString()),
        new Claim("tenantId", customer.TenantId.ToString()),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N"))
    };

            var token = new JwtSecurityToken(
                claims: claims,
                notBefore: now,
                expires: expiresAt,
                signingCredentials: signingCredentials);

            var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var refreshExpiry = now.AddDays(7);

            var refreshEntity = new RefreshTokens
            {
                CustomerUUID = customer.UUID,
                RefreshToken = refreshToken,
                ExpiryAt = refreshExpiry,
                CreatedAt = now,
                IsRevoked = false
            };

            await _refreshTokenRepository.AddAsync(refreshEntity);

            TokenResponseDto tokenResponse = new TokenResponseDto
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                AccessTokenExpiresAtUtc = expiresAt,
                RefreshToken = refreshToken,
                RefreshTokenExpiresAtUtc = refreshExpiry

            };

            return tokenResponse;
        }

        public ClaimsPrincipal? ValidateAccessToken(string accessToken)
        {
            var publicKeyPath = KeyPathResolver.GetPublicKeyPath();

            if (!File.Exists(publicKeyPath))
                throw new InvalidOperationException(
                    $"JWT public key file not found: {publicKeyPath}");

            var publicKeyPem = File.ReadAllText(publicKeyPath);

            using var rsa = RSA.Create();
            rsa.ImportFromPem(publicKeyPem);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new RsaSecurityKey(rsa),
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var principal = tokenHandler.ValidateToken(
                    accessToken,
                    validationParameters,
                    out var validatedToken);

                if (validatedToken is JwtSecurityToken jwtToken)
                {
                    var isValidAlg = jwtToken.Header.Alg.Equals(
                        SecurityAlgorithms.RsaSha256,
                        StringComparison.InvariantCultureIgnoreCase);

                    if (!isValidAlg)
                        return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }


        public async Task<TokenResponseDto> RefreshTokenAsync(string refreshToken)
        {
            var existingToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

            if (existingToken == null)
                throw new Exception("Invalid refresh token");

            if (existingToken.IsRevoked)
                throw new Exception("Token already revoked");

            if (existingToken.ExpiryAt < DateTimeOffset.UtcNow)
                throw new Exception("Refresh token expired");

            // 🔹 Get customer
            var customer = await _masterCustomerRepository.GetByUuidAsync(existingToken.CustomerUUID);

            if (customer == null)
                throw new Exception("Customer not found");

            // 🔥 ROTATE TOKEN
            existingToken.IsRevoked = true;                                         

            await _refreshTokenRepository.UpdateAsync(existingToken);

            var tenant = await _tenantRepository.GetTenantDetailsByIdAsync(customer.TenantId);

            var newTokens = await GenerateCustomerTokenAsync(customer); 


            return newTokens;
        }
    }
}
