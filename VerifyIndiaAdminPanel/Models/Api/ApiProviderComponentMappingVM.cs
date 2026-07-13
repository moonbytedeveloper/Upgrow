using Microsoft.AspNetCore.Mvc.Rendering;
using VerifyIndia.Application.Commands.Api;
using VerifyIndia.Application.Commands.Master;

namespace VerifyIndiaAdminPanel.Models.Api
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
