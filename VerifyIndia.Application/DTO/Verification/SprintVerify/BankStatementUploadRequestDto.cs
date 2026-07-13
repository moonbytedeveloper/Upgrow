using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Verification.SprintVerify
{
    public class BankStatementUploadRequestDto
    {
        //[Required(ErrorMessage = "Request ID is required")]
        [JsonIgnore]
        public string reqid { get; set; }
        [Required(ErrorMessage = "File is required")]
        public IFormFile file { get; set; }
    }
}
