using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.CustomerPanel.QueryResults
{
    public class ApiQueryResult
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

        public string? ReqPayload { get; set; }
    }
}
