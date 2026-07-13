using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Upgrow.Application.Commands.Master;

namespace UpgrowAdminPanel.Models.Master
{
    public class PolicyVersionVM
    {
        public MasterPolicyCommand Command { get; set; } = new();
        
        public List<(string UUID, string Title)> PolicyOptions { get; set; } = new();
        public List<SelectListItem> PolicyList { get; set; } = new();
        public List<SelectListItem> VersionTypeList { get; set; } = new List<SelectListItem>();
    

    }
}
