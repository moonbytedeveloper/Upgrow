using Microsoft.AspNetCore.Mvc.Rendering;
using VerifyIndia.Application.Commands.Master;

namespace UpgrowAdminPanel.Models
{
    public class MasterEmailTemplateVM
    {
        public MasterEmailTemplateCommand EmailTemplate { get; set; } = new();
        public List<SelectListItem> EmailCredentialList { get; set; } = new();
    }
}
