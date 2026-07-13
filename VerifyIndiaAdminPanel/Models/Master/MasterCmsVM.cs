using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VerifyIndia.Application.Commands.Master;

namespace UpgrowAdminPanel.Models.Master
{
    public class MasterCmsVM
    {
        public MasterCMSCommand Command { get; set; } = new();
       
        // For Dropdown Options
        public List<(string UUID, string PageTitle)> CmsOptions { get; set; } = new();

    }
}
