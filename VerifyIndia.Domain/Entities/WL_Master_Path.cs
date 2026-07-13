using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Common;

namespace Upgrow.Domain.Entities
{
    public class WL_Master_Path : BaseEntity
    {
        public decimal Id { get; set; }
        public string UUID { get; set; }
        public string Path { get; set; }

        public bool IsActive { get; set; }
    }
}
