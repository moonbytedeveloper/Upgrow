using Microsoft.AspNetCore.Mvc.Rendering;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.Commands.Website;

namespace UpgrowAdminPanel.Models.Website
{
    public class WebsiteMasterIndustryVM
    {
        public Website_IndustryPointsCommand IndPoints { get; set; } = new();
        public Website_IndustryCasesCommand IndCases { get; set; } = new();

        public Website_MasterIndustryCommand MasterInd { get; set; } = new();
    }
}
