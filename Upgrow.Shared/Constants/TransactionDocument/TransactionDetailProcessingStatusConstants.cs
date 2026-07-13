using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Shared.Constants.TransactionDocument
{
    public static class TransactionDetailProcessingStatusConstants
    {
        public const string Pending =
            "Pending";

        public const string WaitingForCustomerInput =
            "WaitingForCustomerInput";

        public const string Completed =
            "Completed";

        public const string Failed =
            "Failed";

        public const string Processing = 
            "Processing";
    }
}
