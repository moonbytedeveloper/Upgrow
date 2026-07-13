using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.CustomerPanel
{
    public class ApiDto
    {
        public string UUID { get; set; } = default!;
        public string ApiName { get; set; } = default!;
        public string Code { get; set; } = default!;
        public string ShortDescription { get; set; } = default!;
        public int DisplayOrder { get; set; }

        public bool IsPinned { get; set; }
        public bool IsAddedInCart { get; set; }

        public decimal Price { get; set; }

        public bool IsMultiEndpoint { get; set; }

        public string ComponentName { get; set; } = string.Empty;
        public string SubmitUrl { get; set; } = string.Empty;
        public string SubmitHttpMethod { get; set; } = string.Empty;

        public string SecondComponentName { get; set; } = string.Empty;
        public string SecondSubmitUrl { get; set; } = string.Empty;
        public string SecondSubmitHttpMethod { get; set; } = string.Empty;

        public bool IsConsentRequired { get; set; }

        public List<ApiRequestFieldDto> RequestFields { get; set; } = new();
        //    public List<ApiRequestFieldDto> RequestFields { get; set; } = [
        //    new() { Name = "pan", Value = "BHH78RI584" },
        //    new() { Name = "name_as_per_pan", Value = "Pan Holder Name" },
        //    new() { Name = "date_of_birth", Value = "2026-06-11" },
        //    new() { Name = "consent", Value = "y" },
        //    new() { Name = "reason", Value = "test" }
        //];

        public List<ApiSectionDto> Sections { get; set; } = new();
    }

    public class ApiRequestFieldDto
    {
        public string Name { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;
    }
    public class ApiSectionDto
    {
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int Sequence { get; set; }

        public List<ApiFieldDto> Fields { get; set; } = new();
    }

    public class ApiFieldDto
    {
        public string Title { get; set; } = string.Empty;

        public int Sequence { get; set; }
    }
}
