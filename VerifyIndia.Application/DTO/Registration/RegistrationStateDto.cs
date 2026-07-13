using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.Customer;
using VerifyIndia.Application.DTO.DropDown;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.DTO.Verification.SprintVerify;
using VerifyIndia.Application.DTOs.Master;

namespace VerifyIndia.Application.DTO.Registration
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
