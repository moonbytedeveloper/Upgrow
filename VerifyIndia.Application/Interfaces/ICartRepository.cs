using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.DTO.Cart;
using Upgrow.Application.DTO.Transaction;
using Upgrow.Domain.Entities;

namespace Upgrow.Application.Interfaces
{
    public interface ICartRepository
    {
        Task<Master_Cart?> GetPendingCartAsync(
            string verifierUuid);

        Task<List<CartDetail>> GetCartDetailsAsync(
            string cartUuid);

        Task<bool> ExistsInCartAsync(
            string cartUuid,
            string apiUuid);

        Task<List<CartSummaryItemDto>> GetCartSummaryItemsAsync(
            string cartUuid);

        Task AddCartAsync(
            Master_Cart cart);

        Task AddCartDetailAsync(
            CartDetail detail);

        Task UpdateCartAsync(
            Master_Cart cart);

        Task SaveChangesAsync();

        Task<CartDetail?> GetCartDetailAsync(
            string cartUuid,
            string apiUuid);

        Task DeleteCartDetailAsync(
            CartDetail entity);

        Task<List<CartTransactionDetailDto>> GetCartTransactionDetailsAsync(
        string cartUuid);

    }
}
