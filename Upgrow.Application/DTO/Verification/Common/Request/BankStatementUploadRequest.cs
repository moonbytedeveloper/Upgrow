using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.Common.Request
{
    public class BankStatementUploadRequest
    {
        public string reqid { get; set; }
        [Required(ErrorMessage = "File is required")]
        public IFormFile file { get; set; }
    }
}
