using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;

namespace VerifyIndia.Application.Commands.Website
{
    public class KnowledgeHubCommand :IMasterCommand
    {
        public string? UUID { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string? CategoryUUID { get; set; }
        [Required(ErrorMessage = "Required!")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters are allowed")]
        public string? Name { get; set; }
        public string? ImageURL { get; set; }
        public IFormFile? Image { get; set; }
        [NotMapped]
        public string? ImageUrlwithdomain { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string? Description { get; set; }
        public string? LongDescription { get; set; }
        public decimal? SequenceNo { get; set; }
        public bool IsActive { get; set; }
    }
}
