using Microsoft.AspNetCore.Mvc.Rendering;
using Upgrow.Application.Commands.Api;
using Upgrow.Application.Commands.Master;

namespace UpgrowAdminPanel.Models.Api
{
    public class ApiProviderComponentMappingVM
    {
        public ApiProviderComponentMappingCommand ApiComponentMapping { get; set; } = new();
        public IEnumerable<SelectListItem> Api { get; set; } = [];
        public IEnumerable<SelectListItem> Component { get; set; } = [];
        public IEnumerable<ApiProviderMapsItem> ApiProviderMappings { get; set; } = [];
    }

    public class ApiProviderMapsItem
    {
        public string ApiUUID { get; set; } = string.Empty;
        public string ProviderUUID { get; set; } = string.Empty;
    }
}
