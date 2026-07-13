using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
using VerifyIndia.Application;
using VerifyIndia.Application.Commands.AIX;
using VerifyIndia.Application.Commands.Api;
using VerifyIndia.Application.DTO.AIX;


namespace UpgrowAdminPanel.Models.Apix
{
    public class ApiXVersionVM
    {
        public ApixVersionCommand ApixVersionCommand { get; set; } = new();


        [ValidateNever]
        public ApixHeaderCommand Header { get; set; } = new();
       public RequestSchemaCommand RequestSchema { get; set; } = new();
        public ResponseSchemaCommand ResponseSchema { get; set; } = new();

        public ApiXCodeMapperCommand ApiXCodeMapper { get; set; } = new();
        public ApiXLanguageContentCommand LanguageSnippet { get; set; } = new();



        public IEnumerable<SelectListItem> ApiGroup { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> ApiCategory { get; set; } = new List<SelectListItem>();

        public IEnumerable<SelectListItem> ApiMethods { get; set; } = new List<SelectListItem>
        {
              new SelectListItem { Value = "GET",  Text = "GET"  },
              new SelectListItem { Value = "POST", Text = "POST" },
              new SelectListItem { Value = "PUT",  Text = "PUT"  },
              new SelectListItem { Value = "PATCH",  Text = "PATCH"  },
              new SelectListItem { Value = "DELETE", Text = "DELETE" }
        };
        public IEnumerable<SelectListItem> RequestType { get; set; } = new List<SelectListItem>
        {
              new SelectListItem { Value = "multipart/form-date",  Text = "multipart/form-date"  },
              new SelectListItem { Value = "application/json", Text = "application/json" }
              
        };

        public IEnumerable<SelectListItem> DataTypes { get; set; } =
           Constants.DataTypeConstants.All
               .Select(x => new SelectListItem
               {
                   Value = x,
                   Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x)
               })
               .ToList();
        public ICollection<ApixHeaderDto> Headers { get; set; } = new List<ApixHeaderDto>();
        public IEnumerable<SelectListItem> RequestTypes { get; set; } = new List<SelectListItem>();

        [ValidateNever]
        public ReqResSchemaFieldCommand Field { get; set; } = new();

        // Add to ApiXVersionVM
        [ValidateNever]
        public List<ReqResSchemaFieldCommand> Fields { get; set; } = new();
        public IEnumerable<SelectListItem> StatusList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> CodeExampleList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> ResponseSchemaList { get; set; } = new List<SelectListItem>();
        // add using Microsoft.AspNetCore.Mvc.Rendering already present
        public IEnumerable<SelectListItem> ProgrammingLanguages { get; set; } = new List<SelectListItem>();

    }
}

    