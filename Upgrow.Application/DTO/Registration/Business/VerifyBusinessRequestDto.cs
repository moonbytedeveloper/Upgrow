using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Registration.Business
{
    public sealed class VerifyBusinessRequestDto
    {
        public string BusinessTypeUUID
        {
            get;
            set;
        } = string.Empty;

        /// <summary>
        /// CIN / LLPIN
        /// </summary>
        public string? RegistrationNumber
        {
            get;
            set;
        }

        /// <summary>
        /// GST / Shop License / Udyam / Bank Account etc.
        /// </summary>
        public string? VerificationDocumentUUID
        {
            get;
            set;
        }

        public string? VerificationDocumentNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Partnership PAN
        /// </summary>
        public string? PAN
        {
            get;
            set;
        }

        public string? GSTIN
        {
            get;
            set;
        }
    }
}
