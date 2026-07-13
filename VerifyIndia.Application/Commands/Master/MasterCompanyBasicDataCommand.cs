using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VerifyIndia.Application.Commands.Master;

namespace VerifyIndia.Application.Commands.Master
{
    public class MasterCompanyBasicDataCommand : IMasterCommand
    {
        public string? UUID { get; set; }
        [Required(ErrorMessage = "Company name is required!")]
        public string? CompName { get; set; }
        [Required(ErrorMessage = "Company contact number is required!")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Contact number must be exactly 10 digits")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string? Phone { get; set; }
        [Required(ErrorMessage = "Company email is required!")]
        [EmailAddress(ErrorMessage = "Invalid email format. Email must contain '@' and '.'")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Email must be valid with '@' and domain extension")]
        public string? EmailId { get; set; }
        [Required(ErrorMessage = "Google iframe is required!")]
        public string? GoogleMapIframe { get; set; }
        [Required(ErrorMessage = "Company address is required!")]
        public string? Address { get; set; }
        public string? WebsiteLogo { get; set; }
        public string? StickyLogo { get; set; }
        public string? FooterLogo { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsIncremental { get; set; }
        // File upload properties
        public IFormFile? WebsiteLogoImage { get; set; }
        public IFormFile? StickyLogoImage { get; set; }
        public IFormFile? FooterLogoImage { get; set; }
        [NotMapped]
        public string? websiteUrlwithdomain { get; set; }

        [NotMapped]
        public string? stickylogoUrlwithdomain { get; set; }

        [NotMapped]
        public string? footerUrlwithdomain { get; set; }
    }
}