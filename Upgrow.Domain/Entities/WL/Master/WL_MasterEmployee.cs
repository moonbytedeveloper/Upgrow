using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Domain.Entities.WL.Master
{
    public class WL_MasterEmployee : TenantEntity
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? MobileNumber { get; set; }

        public string? EmailId { get; set; }

        public string? GenderUUID { get; set; }

        public string? DepartmentUUID { get; set; }
        public string? RoleUUID { get; set; }

        public string? DesignationUUID { get; set; }
        public string? HonorificUUID { get; set; }

        public DateTime? BirthDate { get; set; }

        public string? CompanyEmailID { get; set; }

        public string? CompanyMobileNumber { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }

        public bool IsLoginAllowed { get; set; }

        public string? Profile_URL { get; set; }

        public string? EmployeeCode { get; set; }
    }
}
