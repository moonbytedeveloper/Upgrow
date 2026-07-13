using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities.Registration
{
    public class Master_BusinessType : BaseEntity
    {
        public string Name
        {
            get;
            set;
        } = string.Empty;

        public string Code
        {
            get;
            set;
        } = string.Empty;

        public decimal DisplayOrder
        {
            get;
            set;
        }
    }
}
