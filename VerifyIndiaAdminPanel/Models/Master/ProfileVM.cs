using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VerifyIndiaAdminPanel.Models.Master
{
    public class ProfileVM
    {
        public string? UUID { get; set; }
        [Required(ErrorMessage = "Required!")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters are allowed")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Required!")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters are allowed")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Required!")]
        public DateTime? BirthDate { get; set; }
        public string? Profile_URL { get; set; }

        [NotMapped]
        public string? ProfileUrlwithdomain { get; set; }
        public string? UserName { get; set; }
        public IFormFile? Profile { get; set; }
    }
}
