using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Verification.SprintVerify.Response
{
    public class PS_PennyDropVerifyResponse
    {
        public string? NwTxnRefId { get; set; }

        public PennyDropDetails? ReqDtls { get; set; }

        public string? NwRespMessg { get; set; }

        public string? TxnRefNo { get; set; }

        public string? NwRespCode { get; set; }

        public string? CustomerName { get; set; }

        public long ReferenceId { get; set; }
    }
    public class PennyDropDetails
    {
        public string? UserTxnRefNo { get; set; }

        public string? TxnType { get; set; }

        public string? ChanTxnRefNo { get; set; }
    }

}

