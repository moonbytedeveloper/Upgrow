using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace UpgrowAdminPanel.Models.Inquiry
{
    public class InquiryGeneralStatusVM
    {
        public string? UUID { get; set; }
        public bool IsActive { get; set; } = true;

        //public List<SelectListItem>? statusList { get; set; } = new List<SelectListItem> {
        //    new SelectListItem { Text = "Open", Value = "1" },
        //    new SelectListItem { Text = "Closed", Value = "0" }
        //};

        [Required(ErrorMessage = "Remark is required.")]
        public string? Remark { get; set; }
    }
}
