using Microsoft.Extensions.Configuration;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.DTO.Payment;
using Upgrow.Application.IServices.Payment;

namespace Upgrow.Application.Services.Registration
{
    public sealed class RazorpayService
        : IRazorpayService
    {
        private readonly IConfiguration
            _configuration;

        public RazorpayService(
            IConfiguration configuration)
        {
            _configuration =
                configuration;
        }

        public async Task<
            RazorpayOrderResponseDto>
            CreateOrderAsync(
                decimal amount,
                string receiptId)
        {
            var keyId =
                _configuration[
                    "Razorpay:KeyId"]!;

            var keySecret =
                _configuration[
                    "Razorpay:KeySecret"]!;

            if (string.IsNullOrWhiteSpace(keyId))
            {
                throw new Exception(
                    "Razorpay KeyId not configured.");
            }

            if (string.IsNullOrWhiteSpace(keySecret))
            {
                throw new Exception(
                    "Razorpay KeySecret not configured.");
            }

            var client =
                new RazorpayClient(
                    keyId,
                    keySecret);

            var options =
                new Dictionary<string, object>
                {
                    {
                        "amount", Convert.ToInt32(amount * 100)
                    },
                    {
                        "currency", "INR"
                    },
                    {
                        "receipt", receiptId
                    },
                    {
                        "payment_capture", 1
                    }
                };

            var order =
                await Task.Run(
                    () =>
                    client.Order.Create(
                        options));

            return new RazorpayOrderResponseDto
            {
                OrderId =
                    order["id"]
                        .ToString(),

                Amount =
                    amount,

                KeyId =
                    keyId
            };
        }

        public bool VerifySignature(
            string orderId,
            string paymentId,
            string signature)
        {
            var keySecret =
                _configuration[
                    "Razorpay:KeySecret"]!;

            var payload =
                $"{orderId}|{paymentId}";

            using var hmac =
                new HMACSHA256(
                    Encoding.UTF8.GetBytes(
                        keySecret));

            var hash =
                hmac.ComputeHash(
                    Encoding.UTF8.GetBytes(
                        payload));

            var generatedSignature =
                BitConverter
                    .ToString(hash)
                    .Replace("-", "")
                    .ToLower();

            return generatedSignature ==
                   signature.ToLower();
        }

        public string GetKeyId()
        {
            return _configuration[
                "Razorpay:KeyId"]!;
        }
    }
}
