using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Shared.Constants
{
    public static class CartStatusConstants
    {
        /// <summary>
        /// Cart is active and can be modified.
        /// </summary>
        public const string PENDING = "PENDING";

        /// <summary>
        /// Transaction has been initiated from this cart.
        /// Cart can no longer be modified.
        /// </summary>
        public const string CHECKOUT = "CHECKOUT";

        /// <summary>
        /// All APIs removed or cart abandoned.
        /// </summary>
        public const string DEACTIVATED = "DEACTIVATED";
    }
}
