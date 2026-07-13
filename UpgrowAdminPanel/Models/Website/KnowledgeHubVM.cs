using Microsoft.AspNetCore.Mvc.Rendering;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.Commands.Website;

namespace UpgrowAdminPanel.Models.Website
{
    public class KnowledgeHubVM
    {
        public KnowledgeHubCommand KnowledgeHub { get; set; } = new();
        public List<SelectListItem> Categories { get; set; } = [];  
    }
}
