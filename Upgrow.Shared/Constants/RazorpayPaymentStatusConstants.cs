using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Shared.Constants
{
    public static class RazorpayPaymentStatusConstants
    {
        /// <summary>
        /// Payment initiated but not completed.
        /// </summary>
        public const string Pending = "Pending";

        /// <summary>
        /// Payment verified successfully.
        /// </summary>
        public const string Success = "Success";

        /// <summary>
        /// Payment verification failed.
        /// </summary>
        public const string Failed = "Failed";

        public const string Cancelled = "CANCELLED";
    }
}
