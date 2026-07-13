using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities
{
    public class MasterDosDontsDocument : BaseEntity
    {

        public string Title { get; set; }
            = string.Empty;

        public string VersionNo { get; set; }
            = string.Empty;

        public string Status { get; set; }
            = string.Empty;

        public DateTimeOffset? PublishedAt { get; set; }

        public DateTimeOffset CreatedAt { get; set; }


        public virtual ICollection<Master_DosDonts>
            DosDontsItems
        { get; set; }
                = new List<Master_DosDonts>();
    }
}
