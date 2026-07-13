using AuthenticateIndia.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.DTO.Cart;
using Upgrow.Application.DTO.Transaction;
using Upgrow.Application.Interfaces;
using Upgrow.Domain;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Infrastructure.Repositories.Cart
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;

        public CartRepository(
            AppDbContext context)
        {
            _context = context;
        }

        public async Task<Master_Cart?> GetPendingCartAsync(
            string verifierUuid)
        {
            return await _context.Master_Carts
                .FirstOrDefaultAsync(x =>
                    x.VerifierUUID == verifierUuid
                    && x.Status == CartStatusConstants.PENDING
                    && x.IsActive);
        }

        public async Task<List<CartDetail>>
            GetCartDetailsAsync(
                string cartUuid)
        {
            return await _context.CartDetails
                .Where(x =>
                    x.CartUUID == cartUuid
                    && x.IsActive)
                .ToListAsync();
        }

        public async Task<bool> ExistsInCartAsync(
            string cartUuid,
            string apiUuid)
        {
            return await _context.CartDetails
                .AnyAsync(x =>
                    x.CartUUID == cartUuid
                    && x.ApiUUID == apiUuid
                    && x.IsActive);
        }

        public async Task<List<CartSummaryItemDto>>
            GetCartSummaryItemsAsync(
                string cartUuid)
        {
            return await
            (
                from cartDetail
                    in _context.CartDetails

                join api
                    in _context.Master_Api
                    on cartDetail.ApiUUID
                    equals api.UUID

                where
                    cartDetail.CartUUID == cartUuid
                    &&
                    cartDetail.IsActive
                    &&
                    api.IsActive

                orderby api.DisplayOrder

                select new CartSummaryItemDto
                {
                    ApiUUID =
                        cartDetail.ApiUUID,

                    ApiName =
                        api.ApiName,

                    ApiCharge =
                        cartDetail.ApiCharge,

                    IsConsentBased =
                        cartDetail.IsConsentBased
                }
            )
            .AsNoTracking()
            .ToListAsync();
        }

        public async Task AddCartAsync(
            Master_Cart cart)
        {
            await _context.Master_Carts
                .AddAsync(cart);
        }

        public async Task AddCartDetailAsync(
            CartDetail detail)
        {
            await _context.CartDetails
                .AddAsync(detail);
        }

        public Task UpdateCartAsync(
            Master_Cart cart)
        {
            _context.Master_Carts.Update(cart);

            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<CartDetail?> GetCartDetailAsync(
            string cartUuid,
            string apiUuid)
        {
            return await _context
                .CartDetails
                .FirstOrDefaultAsync(x =>
                    x.CartUUID == cartUuid
                    &&
                    x.ApiUUID == apiUuid
                    &&
                    x.IsActive);
        }

        public Task DeleteCartDetailAsync(
            CartDetail entity)
        {
            entity.IsActive = false;

            _context
                .CartDetails
                .Update(entity);

            return Task.CompletedTask;
        }

        public async Task<List<CartTransactionDetailDto>> GetCartTransactionDetailsAsync(
            string cartUuid)
        {
            return await
                (from cartDetail in _context.CartDetails

                 join api in _context.Master_Api
                     on cartDetail.ApiUUID equals api.UUID

                 where cartDetail.CartUUID == cartUuid
                       && cartDetail.IsActive

                 select new CartTransactionDetailDto
                 {
                     ApiUUID =
                         cartDetail.ApiUUID,

                     VerificationCode =
                         api.Code,

                     PricingUUID =
                         cartDetail.PricingUUID,

                     ApiCharge =
                         cartDetail.ApiCharge,

                     ReqPayload =
                         cartDetail.ReqPayload,

                     IsMultipleEndPoint =
                         cartDetail.IsMultipleEndPoint,

                     IsConsentBased =
                         cartDetail.IsConsentBased
                 })
                .ToListAsync();
        }
    }
}
