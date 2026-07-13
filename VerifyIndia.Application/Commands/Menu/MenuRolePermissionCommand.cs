using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;

namespace VerifyIndia.Application.Commands.Menu
{
    public class MenuRolePermissionCommand : IMasterCommand
    {
        public string? UUID { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string RoleUUID { get; set; } = null!;

        public string? PermissionUUID { get; set; }

        public bool IsActive { get; set; }
    }
}
