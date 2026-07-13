namespace Upgrow.Domain.Entities
{
    public class Master_Customer : BaseEntity
    {
        
        public string? FName { get; set; }          
        public string? MName { get; set; }       
        public string? LName { get; set; }          
        public string? Mobile { get; set; }           
        public string? Email { get; set; }
        public string? ACType { get; set; }
        public string? ProfileURL { get; set; }

        public bool IsAgent { get; set; }
        public bool IsAgentHead { get; set; }
        public string? ACLink { get; set; }
        public decimal TenantId { get; set; } 
        public string? StateUUID { get; set; }        
        public string? CityUUID { get; set; }          
        public string? IndustryUUID { get; set; }    
        public string? ReferralCode { get; set; }      

        public bool IsRegistered { get; set; }        
        public bool IsAccessAllowed { get; set; }       
        public DateTimeOffset? RegTimeStamp { get; set; } 
        public bool IsRegisteredAsX { get; set; }
        public string? XApiKey { get; set; }
        public string? CurrentStep { get; set; }

        public bool IsAadhaarVerified { get; set; }

        public string? AadhaarHash { get; set; }

        public string? BusinessTypeUUID { get; set; }

        public bool? IsBusinessVerified { get; set; }

        public bool? IsDirectorSelected { get; set; }
    }   
}
