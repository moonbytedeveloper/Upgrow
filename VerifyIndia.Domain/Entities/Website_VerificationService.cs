using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Domain.Entities
{
    public class Website_VerificationService : BaseEntity
    {    
        public string? Title { get; set; }

        public string? ServiceCategoryUUID { get; set; }

    
    }
}
