using Microsoft.AspNetCore.Mvc.Rendering;
using VerifyIndia.Application.Commands.Api;

namespace VerifyIndiaAdminPanel.Models.Api
{
    public class ApiEndpointVM
    {
        public ApiEndpointCommand ApiEndpoint { get; set; } = new();
        public IEnumerable<SelectListItem> Api { get; set; } = [];
        public IEnumerable<SelectListItem> HttpMethods { get; set; } = new List<SelectListItem>
        {
              new SelectListItem { Value = "GET",  Text = "GET"  },
              new SelectListItem { Value = "POST", Text = "POST" },
              new SelectListItem { Value = "PUT",  Text = "PUT"  },
              new SelectListItem { Value = "PATCH",  Text = "PATCH"  },
              new SelectListItem { Value = "DELETE", Text = "DELETE" }
        };
        public IEnumerable<ApiProvidermapsItem> ApiProviderMapping { get; set; } = [];
    }
    public class ApiProvidermapsItem
    {
        public string ApiUUID { get; set; } = string.Empty;
        public string ProviderUUID { get; set; } = string.Empty;
    }
}
