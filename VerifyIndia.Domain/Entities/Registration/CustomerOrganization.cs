using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities.Registration
{
    public class CustomerOrganization : BaseEntity
    {
        public string CustomerUUID
        {
            get;
            set;
        } = string.Empty;

        public string BusinessTypeUUID
        {
            get;
            set;
        } = string.Empty;

        public string? BusinessName
        {
            get;
            set;
        }

        public string? BusinessRegistrationNumber
        {
            get;
            set;
        }
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

        public string? GSTIN
        {
            get;
            set;
        }

        public string? PAN
        {
            get;
            set;
        }

        public string? SelectedDirectorName
        {
            get;
            set;
        }

        public string? SelectedDirectorDIN
        {
            get;
            set;
        }

        public string VerificationStatus
        {
            get;
            set;
        }

        public DateTimeOffset CreatedAt
        {
            get;
            set;
        }

        public DateTimeOffset? UpdatedAt
        {
            get;
            set;
        }
    }
}
