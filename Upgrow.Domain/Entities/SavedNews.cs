using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Common;

namespace Upgrow.Domain.Entities
{
    public class SavedNews : BaseEntity
    { 
        public string? NewsUUID { get; set; }
        public string? CustomerUUID { get; set; }
       
    }
}
