using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Registration
{
    public sealed class DosDontsDocumentDto
    {
        public string DocumentUUID { get; set; }
            = string.Empty;

        public string VersionNo { get; set; }
            = string.Empty;

        public bool IsAccepted { get; set; }
        public List<DosDontsItemDto> Dos { get; set; }
            = [];

        public List<DosDontsItemDto> Donts { get; set; }
            = [];
        
    }
}
