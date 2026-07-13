using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Registration.Business
{
    public sealed class CreateBusinessOrderRequestDto
    {
        /// <summary>
        /// Private Ltd / LLP / Proprietorship / Partnership
        /// </summary>
        public string BusinessTypeUUID { get; set; } = string.Empty;

        /// <summary>
        /// CIN / LLPIN
        /// </summary>
        public string? BusinessRegistrationNumber { get; set; }

        /// <summary>
        /// GST / Shop License / Udyam / Bank Account
        /// </summary>
        public string? VerificationDocumentUUID { get; set; }

        public string? VerificationDocumentNumber { get; set; }

        /// <summary>
        /// Partnership PAN
        /// </summary>
        public string? PAN { get; set; }

        /// <summary>
        /// Optional GST
        /// </summary>
        public string? GSTIN { get; set; }
    }
}
