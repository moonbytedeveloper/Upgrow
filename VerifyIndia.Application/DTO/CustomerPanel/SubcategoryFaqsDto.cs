using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.CustomerPanel
{
    public class FaqSimpleDto
    {
        public string? UUID { get; set; }
        public string? Question { get; set; }
        public string? Answer { get; set; }
    }
    public class SubcategoryFaqsDto
    {
        public string? UUID { get; set; }
        public string? Title { get; set; }
        /// <summary>
        /// Image path (will be resolved to absolute URL by the controller / caller)
        /// </summary>
        public string? Image { get; set; }
        public List<FaqSimpleDto> Faqs { get; set; } = new List<FaqSimpleDto>();

    }
}
