using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.Common.Response
{
    public class PennyDropVerifyResponse
    {
        public string? NwTxnRefId { get; set; }

        public PennyDropRequestDetails? ReqDtls { get; set; }

        public string? NwRespMessg { get; set; }

        public string? TxnRefNo { get; set; }

        public string? NwRespCode { get; set; }

        public string? CustomerName { get; set; }

        public long ReferenceId { get; set; }
    }
    public class PennyDropRequestDetails
    {
        public string? UserTxnRefNo { get; set; }

        public string? TxnType { get; set; }

        public string? ChanTxnRefNo { get; set; }
    }
}

