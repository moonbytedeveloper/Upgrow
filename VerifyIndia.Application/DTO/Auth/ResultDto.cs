using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Auth
{
    public class ResultDto
    {
        public string? UUID { get; set; }

        public string? FullName { get; set; }
        public string? ProfilePic { get; set; }

        public string? RoleUUID { get; set; }

        public string? IP { get; set; }
        public DateTime? LastLogin { get; set; }

        public string? EmployeeCode { get; set; }

        public string? Honorific { get; set; }
    }
}
