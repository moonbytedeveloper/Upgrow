using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.Payment;

namespace VerifyIndia.Application.IServices.Payment
{
    public interface IRazorpayService
    {
        Task<RazorpayOrderResponseDto>
            CreateOrderAsync(
                decimal amount,
                string receiptId);

        bool VerifySignature(
            string orderId,
            string paymentId,
            string signature);

        string GetKeyId();
    }
}
