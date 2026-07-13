using AuthenticateIndia.Shared.Constants;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Common;

namespace VerifyIndia.Application.Commands.Master
{
    public class AppSettingCommand : IMasterCommand 
    {
        public string? UUID { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string Key { get; set; } = null!;
        [Required(ErrorMessage = "Required!")]
        public string? Value { get; set; }
        [Required(ErrorMessage = "Required!")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public List<SelectListItem> KeyList { get; set; } =
    AppSettingKeys.All
        .Select(x => new SelectListItem(x, x))
        .ToList();
    }
}
