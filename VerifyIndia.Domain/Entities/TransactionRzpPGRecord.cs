namespace VerifyIndia.Domain.Entities
{
    public class TransactionRzpPGRecord
        : BaseEntity
    {
        public string TransactionUUID { get; set; }

        /// <summary>
        /// Razorpay Order Id
        /// Example:
        /// order_Q1AbCdEfGhIjKl
        /// </summary>
        public string OrderId { get; set; }

        public decimal OrderAmount { get; set; }

        public DateTimeOffset OrderCreatedAt { get; set; }

        /// <summary>
        /// Created / Paid / Failed
        /// </summary>
        public string OrderStatus { get; set; }

        /// <summary>
        /// Razorpay Payment Id
        /// Example:
        /// pay_Q1AbCdEfGhIjKl
        /// </summary>
        public string? PaymentId { get; set; }

        public DateTimeOffset? PaymentCreatedAt { get; set; }

        /// <summary>
        /// UPI / CARD / NETBANKING / WALLET
        /// </summary>
        public string? PaymentMethod { get; set; }

        public string? Bank { get; set; }

        public string? Vpa { get; set; }

        public string? Wallet { get; set; }

        /// <summary>
        /// Pending / Success / Failed
        /// </summary>
        public string PaymentStatus { get; set; }

        /// <summary>
        /// Razorpay Fee
        /// </summary>
        public decimal? Fee { get; set; }

        /// <summary>
        /// GST charged by Razorpay
        /// </summary>
        public decimal? Tax { get; set; }

        public string? ErrorCode { get; set; }

        public string? ErrorDescription { get; set; }

        /// <summary>
        /// Only one record per transaction should be current
        /// </summary>
        public bool IsCurrent { get; set; }
    }
}