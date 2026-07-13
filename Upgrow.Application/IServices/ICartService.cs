using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands;
using Upgrow.Application.DTO;
using Upgrow.Application.DTO.Cart;
using Upgrow.Application.DTO.Transaction;
using Upgrow.Application.IServices.Master;

namespace Upgrow.Application.IServices
{
    public interface ICartService
    {
        Task<string> AddToCartAsync(
            AddToCartRequest request,
            string verifierUuid);

        Task<CartSummaryDto?> GetCartSummaryAsync(
            string verifierUuid);

        Task RemoveApiAsync(
            string verifierUuid,
            string apiUuid);

        Task<SaveConsentDetailsResponseDto> SaveConsentDetailsAsync(
            string verifierUuid,
            SaveConsentDetailsRequest request);

        Task<GetPaymentOptionsResponseDto> GetPaymentOptionsAsync(
            string verifierUuid);
    }
}
