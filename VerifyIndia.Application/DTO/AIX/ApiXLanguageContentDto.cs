using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.AIX
{
    public class ApiXLanguageContentDto
    {
        public string? UUID { get; set; }
        public string? ApiXVersionUUID { get; set; }
        public string? LanguageUUID { get; set; }
        public string? LanguageContent { get; set; }
     
        public bool IsActive { get; set; }
    }
}