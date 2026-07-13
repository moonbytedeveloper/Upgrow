using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.SprintVerify.Response
{
    public class PS_MCAResponse
    {
        public string? company_id { get; set; }

        public string? company_name { get; set; }

        public string? company_type { get; set; }

        public PS_McaDetails? details { get; set; }
    }
    public class PS_McaDetails
    {
        public PS_McaCompanyInfo? company_info { get; set; }

        public List<PS_McaDirector>? directors { get; set; }
    }
    public class PS_McaCompanyInfo
    {
        public string? company_status { get; set; }

        public string? date_of_incorporation { get; set; }

        public string? registered_address { get; set; }

        public string? authorized_capital { get; set; }

        public string? paid_up_capital { get; set; }
    }

    public class PS_McaDirector
    {
        public string? director_name { get; set; }
    }
}

