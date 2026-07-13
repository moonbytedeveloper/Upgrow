using Microsoft.AspNetCore.Mvc.Rendering;
using Upgrow.Application.Commands.Master;

namespace UpgrowAdminPanel.Models.Master
{
    public class MasterCareerVM
    {
        public MasterCareerCommand Career { get; set; } = new();
        public List<SelectListItem> Department { get; set; } = [];
        public List<SelectListItem> Country { get; set; } = new();
        public List<SelectListItem> City { get; set; } = new();
        public List<SelectListItem> State { get; set; } = new();
        public List<SelectListItem> JobType { get; set; } = new();
        public List<SelectListItem> Skills { get; set; } = new();
    }
}
