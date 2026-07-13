namespace VerifyIndia.Application.DTO.Inquiry
{
    public class InquiryGeneralDto
    {
        public string? UUID { get; set; }
        public string? FName { get; set; }
        public string? MName { get; set; }
        public string? LName { get; set; }
        public string? EmailId { get; set; }
        public string? PhoneNo { get; set; }
        public string? Message { get; set; }
        public bool IsActive { get; set; } = true;
        public string? Remark { get; set; }
        public string? ActionTakenBy { get; set; }
        public string? FullName { get; set; }

        // Agent
        public bool? SalesExperience { get; set; }
        public string? SalesExperienceDescription { get; set; }
        public string? HasExistingClients { get; set; }

        // WhiteLabel / Distributor
        public string? CompanyName { get; set; }
        public string? CompanyWebsite { get; set; }
        public string? BusinessType { get; set; }
        public string? PreferredDomainName { get; set; }
        public string? ExpectedRetailers { get; set; }

        // Shared location
        public string? StateUUID { get; set; }
        public string? CityUUID { get; set; }

        // Career
        public string? JobPosition { get; set; }
        public string? Experience { get; set; }
        public string? Qualification { get; set; }
        public string? Resume { get; set; }

        public bool? IsConvertedToAgent { get; set; }
        public bool? IsStatusClosed { get; set; }

    }
}