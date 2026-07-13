using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticateIndia.Shared.Constants
{
    public static class TransactionStatusConstants
    {
        public const string Initiated =
            "Initiated";

        public const string ConsentPending =
            "ConsentPending";

        public const string ConsentApproved =
            "ConsentApproved";

        public const string ConsentRejected =
            "ConsentRejected";

        public const string ReadyForExecution =
            "ReadyForExecution";

        public const string Cancelled =
            "Cancelled";
    }
}
