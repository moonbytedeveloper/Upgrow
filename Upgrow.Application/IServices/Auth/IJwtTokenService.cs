using System.Security.Claims;
using Upgrow.Application.DTO.Auth;
using Upgrow.Application.DTO.Customer;
using Upgrow.Domain.Entities;

namespace Upgrow.Application.IServices.Auth
{
    public interface IJwtTokenService
    {
      public Task<TokenResponseDto> GenerateCustomerTokenAsync(Master_Customer customer);
      public ClaimsPrincipal ValidateAccessToken(string accessToken);
      public Task<TokenResponseDto> RefreshTokenAsync(string refreshToken);
    }
}
