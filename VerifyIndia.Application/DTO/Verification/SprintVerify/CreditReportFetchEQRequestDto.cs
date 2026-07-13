using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.SprintVerify
{
    public class CreditReportFetchEQRequestDto
    {
        public string refid { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string name { get; set; }
        [Required(ErrorMessage = "Mobile is required")]
        public string mobile { get; set; }
        [Required(ErrorMessage = "Document_Id is required")]
        public string document_id{ get; set; }
        public string dateofbirth { get; set; }
        public string address { get; set; }
        public string pincode { get; set; }
    }
    
}
