using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.CustomerPanel.QueryResults
{
    public class ApiSectionResultDto
    {
        public string ApiUUID { get; set; } = string.Empty;

        public string SectionUUID { get; set; } = string.Empty;

        public string SectionTitle { get; set; } = string.Empty;

        public string SectionDescription { get; set; } = string.Empty;

        public int SectionSequence { get; set; }

        public string? FieldTitle { get; set; } = string.Empty;

        public int? FieldSequence { get; set; }
    }
}
