using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Verification.SprintVerify.Response
{
    public class PS_FuelPriceFetchResponse
    {
        public string? name { get; set; }

        public decimal? petrolPrice { get; set; }

        public string? petrolChange { get; set; }

        public decimal? dieselPrice { get; set; }

        public string? dieselChange { get; set; }

        public string? priceDate { get; set; }

        public string? citystate { get; set; }

        public string? type { get; set; }

        public string? code { get; set; }
    }
}
