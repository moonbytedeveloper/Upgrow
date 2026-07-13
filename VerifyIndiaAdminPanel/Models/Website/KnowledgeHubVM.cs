using Microsoft.AspNetCore.Mvc.Rendering;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.Commands.Website;

namespace UpgrowAdminPanel.Models.Website
{
    public class KnowledgeHubVM
    {
        public KnowledgeHubCommand KnowledgeHub { get; set; } = new();
        public List<SelectListItem> Categories { get; set; } = [];  
    }
}
