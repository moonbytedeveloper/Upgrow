using Microsoft.AspNetCore.Mvc.Rendering;
using VerifyIndia.Application.Commands.Master;

namespace UpgrowAdminPanel.Models.Master
{
    public class MasterEmployeeVM
    {
        public MasterEmployeeCommand Employee { get; set; } = new();
        public string? PasswordPolicyDescription { get; set; } 
        public IEnumerable<SelectListItem> Gender { get; set; } = [];
        public IEnumerable<SelectListItem> Department { get; set; } = [];
        public IEnumerable<SelectListItem> Role { get; set; } = [];
        public IEnumerable<SelectListItem> Honorific { get; set; } = [];
        public IEnumerable<SelectListItem> Designation { get; set; } = [];
    }
}
