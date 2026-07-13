using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.DTO.Customer;
using Upgrow.Application.DTO.DropDown;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.DTO.Verification.SprintVerify;
using Upgrow.Application.DTOs.Master;

namespace Upgrow.Application.DTO.Registration
{
    public sealed class RegistrationStateDto
    {
        public string CurrentStep { get; set; } = string.Empty;
        public decimal AadhaarVerificationFee { get; set; }
        public bool IsAadhaarPaymentDone { get; set; }
        public bool IsEmailVerified { get; set; }
        public string AadhaarNumber { get; set; } = string.Empty;
        public CustomerDtoForApi Customer { get; set; } = new();

        public List<MasterDropDownDto> States { get; set; } = [];

        public List<MasterDropDownDto> Cities { get; set; } = [];

        public List<MasterDropDownDto> BusinessCategories { get; set; } = [];

        //public RegistrationChallengeDto? Challenge { get; set; } = new();

        public List<PolicyListDto> Policies { get; set; } = [];

        public DosDontsDocumentDto? DosDonts { get; set; } = new();

        public bool IsRegistered { get; set; }

        public bool IsPolicySigned { get; set; }

        public string? SignId { get; set; }

        public string? Signature { get; set; }

        public DateTimeOffset? SignedAt { get; set; }
        public bool IsVideoKycCompleted { get; set; }

        public RegistrationBusinessStateDto Business { get; set; } = new();
    }

    public sealed class RegistrationBusinessStateDto
    {
        public string? BusinessTypeUUID
        {
            get;
            set;
        }

        public string? BusinessTypeName
        {
            get;
            set;
        }

        public bool? IsBusinessVerified
        {
            get;
            set;
        }

        public bool? IsDirectorSelected
        {
            get;
            set;
        }
    }
}
