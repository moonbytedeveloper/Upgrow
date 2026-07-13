using Microsoft.AspNetCore.Mvc.Rendering;
using Upgrow.Application.Commands.Api;
using Upgrow.Application.Commands.Master;

namespace UpgrowAdminPanel.Models.Master
{
    public class MasterApiVM
    {
        public ApiEndpointCommand ApiEndpoint { get; set; } = new();
        public ApiEndpointCommand SecondApiEndpoint { get; set; } = new();
        public MasterApiCommand Api { get; set; }
        public List<SelectListItem> ApiCategoryList { get; set; } = new();
        public List<SelectListItem> ComponentList { get; set; } = new();
        public List<ProviderRowVM> ProviderMappings { get; set; } = new();

        // Selected components
        public string SelectedComponentUUID { get; set; }

        public string SelectedSecondComponentUUID { get; set; }

        public IEnumerable<SelectListItem> HttpMethods { get; set; } = new List<SelectListItem>
        {
              new SelectListItem { Value = "GET",  Text = "GET"  },
              new SelectListItem { Value = "POST", Text = "POST" },
              new SelectListItem { Value = "PUT",  Text = "PUT"  },
              new SelectListItem { Value = "PATCH",  Text = "PATCH"  },
              new SelectListItem { Value = "DELETE", Text = "DELETE" }
        };
    }
    public class ProviderRowVM
    {
        public string? ProviderUUID { get; set; }

        public string? ProviderName { get; set; }

        public bool SupportsApi { get; set; }

        public bool IsActive { get; set; }

        public decimal? Priority { get; set; }
    }
}
