using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Registration.Business
{
    public sealed class CompleteBusinessVerificationRequestDto
    {
        public string SessionUUID
        {
            get;
            set;
        } = string.Empty;

        public string RazorpayOrderId
        {
            get;
            set;
        } = string.Empty;

        public string RazorpayPaymentId
        {
            get;
            set;
        } = string.Empty;

        public string RazorpaySignature
        {
            get;
            set;
        } = string.Empty;
    }
}
