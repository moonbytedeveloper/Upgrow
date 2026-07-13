using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Verification.Common.Response
{
    public class MCAResponse
    {
        public string? company_id { get; set; }

        public string? company_name { get; set; }

        public string? company_type { get; set; }

        public McaDetails? details { get; set; }
    }
    public class McaDetails
    {
        public McaCompanyInfo? company_info { get; set; }

        public List<McaDirector>? directors { get; set; }
    }
    public class McaCompanyInfo
    {
        public string? company_status { get; set; }

        public string? date_of_incorporation { get; set; }

        public string? registered_address { get; set; }

        public string? authorized_capital { get; set; }

        public string? paid_up_capital { get; set; }
    }

    public class McaDirector
    {
        public string? director_name { get; set; }
    }
}
