using Microsoft.AspNetCore.Mvc.Rendering;
using Upgrow.Application.Commands.Api;
using Upgrow.Application.Commands.Master;

namespace UpgrowAdminPanel.Models
{
    public class ApiProviderMappingVM
    {
        public ApiProviderMappingCommand ApiProvider { get; set; } = new();
        public IEnumerable<SelectListItem> Api { get; set; } = [];
        public IEnumerable<SelectListItem> Provider { get; set; } = [];
        // Holds the relationships between APIs and Providers so the client can filter provider dropdown
        public IEnumerable<ApiProviderMapItem> ApiProviderMappings { get; set; } = [];
        public Dictionary<string, bool> IsProviderSwitchable { get; set; } = new();
    }
    public class ApiProviderMapItem
    {
        public string ApiUUID { get; set; } = string.Empty;
        public string ProviderUUID { get; set; } = string.Empty;
    }
}

