 using System.ComponentModel.DataAnnotations;
 
namespace VerifyIndia.Application.Commands.Master
{
    public class MasterMenuCommand : IMasterCommand
    {
        public string? UUID { get; set; }

        [Required(ErrorMessage = "Required!")]
        [StringLength(50, ErrorMessage = "Name cannot be greater than 50 characters please validate the name")]
        public string? MenuName { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9\s\-\ _\.]+$", ErrorMessage = "Invalid icon format")]
        public string? MenuIcon { get; set; }

        [Required(ErrorMessage = "Required!")]
        public decimal? MenuLevel { get; set; }
        public string? MainParentUUID { get; set; }
        public string? SubParentUUID { get; set; }
        public string? PermissionUUID { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string? Url { get; set; }
        public bool? IsParent { get; set; }
        public bool? IsActive { get; set; }
        [Required(ErrorMessage = "Required!")]
        public decimal? Sequence { get; set; }
    }
}
