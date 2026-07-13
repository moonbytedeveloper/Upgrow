using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands;
using VerifyIndia.Application.DTO;
using VerifyIndia.Application.DTO.Cart;
using VerifyIndia.Application.DTO.Transaction;
using VerifyIndia.Application.IServices.Master;

namespace VerifyIndia.Application.IServices
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
