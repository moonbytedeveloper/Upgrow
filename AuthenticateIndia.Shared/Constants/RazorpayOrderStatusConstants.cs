using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticateIndia.Shared.Constants
{
    public static class RazorpayOrderStatusConstants
    {
        /// <summary>
        /// Razorpay order created successfully.
        /// Payment not completed yet.
        /// </summary>
        public const string Created = "Created";

        /// <summary>
        /// Payment completed successfully.
        /// </summary>
        public const string Paid = "Paid";

        /// <summary>
        /// Order failed.
        /// </summary>
        public const string Failed = "Failed";

        public const string Cancelled = "CANCELLED";
    }
}
