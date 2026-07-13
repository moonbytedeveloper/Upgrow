using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities;

namespace Upgrow.Domain.Entities
{
    public class Website_VerificationService : BaseEntity
    {    
        public string? Title { get; set; }

        public string? ServiceCategoryUUID { get; set; }

    
    }
}
