using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Api
{
    public class ApiXVersionDto
    {
        public string? UUID { get; set; }
        public string? ApiXCategoryUUID { get; set; }
        public string? VersionNo { get; set; }
        public string? ApiMethod { get; set; }
        public string? ApiPath { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsRequestBodyRequired { get; set; }
        public int SequenceNo { get; set; }
    }
}
