using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.Commands.Master
{
    public class MasterCustomerCommand : IMasterCommand
    {
        public string? UUID { get; set; }
        public string? FName { get; set; }
        public string? MName { get; set; }
        public string? LName { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public string? ACType { get; set; }

        public bool IsAgent { get; set; }
        public bool IsAgentHead { get; set; }
        public string? ACLink { get; set; }
        public decimal TenantId { get; set; }
        public string? CityUUID { get; set; }
        public string? IndustryUUID { get; set; }
        public string? ReferralCode { get; set; }
        public bool IsRegistered { get; set; }
        public bool IsAccessAllowed { get; set; }
        public DateTimeOffset? RegTimeStamp { get; set; }
        public bool IsRegisteredAsX { get; set; }
        public string? XApiKey { get; set; }
        public string? CurrentStep { get; set; }
        public bool IsActive { get; set; }
    }
}
