using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Transaction
{
    public class GetPaymentOptionsResponseDto
    {
        public string?AadhaarNumber { get; set; }
        public string? MaskedAadhaarNumber { get; set; }

        public string? ConsentMobileNumber { get; set; }

        public List<PaymentMethodDto>
            PaymentMethods
        { get; set; }
            = new();
    }

    public class PaymentMethodDto
    {
        public string Code { get; set; }
            = string.Empty;

        public string Title { get; set; }
            = string.Empty;

        public bool Enabled { get; set; }

        public decimal AvailableCredits { get; set; }

        public CreditSummaryDto CreditSummary
        { get; set; }
            = new();

        public PaymentSummaryDto PaymentSummary
        { get; set; }
            = new();
    }

    public class CreditSummaryDto
    {
        public decimal ServiceCredits { get; set; }

        public decimal ConsentCredits { get; set; }

        public decimal TotalCredits { get; set; }
    }

    public class PaymentSummaryDto
    {
        public decimal Subtotal { get; set; }

        public decimal GstPercentage { get; set; }

        public decimal GstAmount { get; set; }

        public decimal TotalPayable { get; set; }
    }
}
