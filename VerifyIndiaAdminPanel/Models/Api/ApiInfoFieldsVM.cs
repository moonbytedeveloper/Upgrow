using Microsoft.AspNetCore.Mvc.Rendering;
using VerifyIndia.Application.Commands.Api;

namespace UpgrowAdminPanel.Models.Api
{
    public class ApiInfoFieldsVM
    {
        public ApiInfoFieldsCommand ApiInfoFields { get; set; } = new();
        public IEnumerable<SelectListItem> Api { get; set; } = [];

        public IEnumerable<SelectListItem> Section { get; set; } = [];

    }
}
