using VerifyIndia.Application.DTO.Customer;

namespace UpgrowAdminPanel.Models.Master
{
    public class UserDetailsVM
    {
        public string PageTitle { get; set; } = string.Empty;

        public string BackUrl { get; set; } = string.Empty;

        public MasterCustomerDto User { get; set; } = new();
        public string Type { get; set; }
    }
}
