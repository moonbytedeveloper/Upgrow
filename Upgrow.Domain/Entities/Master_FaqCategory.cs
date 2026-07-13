using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Common;

namespace Upgrow.Domain.Entities.Auth
{
    public class Master_FAQCategory : IMasterEntity
    {
        public decimal Id { get; set; }
        public string UUID { get; set; }
        public string Title { get; set; }
        public string? Icon { get; set; }
        public string? ShortDescription { get; set; }
        public bool IsActive { get; set; }
    }
}
