using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Shared.Constants.TransactionDocument
{
    public static class TransactionProcessingStatusConstants
    {
        public const string Pending =
            "Pending";

        public const string Processing =
            "Processing";

        public const string Completed =
            "Completed";

        public const string PartiallyCompleted =
            "PartiallyCompleted";

        public const string Failed =
            "Failed";

        public const string PendingUserAction = 
            "PendingUserAction";
    }
}
