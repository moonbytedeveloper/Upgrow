using System.Security.Claims;
using VerifyIndia.Application.DTO.Auth;
using VerifyIndia.Application.DTO.Customer;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Application.IServices.Auth
{
    public interface IJwtTokenService
    {
      public Task<TokenResponseDto> GenerateCustomerTokenAsync(Master_Customer customer);
      public ClaimsPrincipal ValidateAccessToken(string accessToken);
      public Task<TokenResponseDto> RefreshTokenAsync(string refreshToken);
    }
}
