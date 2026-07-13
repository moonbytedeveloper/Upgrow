using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Common;

namespace Upgrow.Domain.Entities
{
    public class Master_DosDonts
        : BaseEntity
    {
        /// <summary>
        /// Parent Do's & Don'ts document.
        /// </summary>
        public string DocumentUUID { get; set; }
            = string.Empty;

        /// <summary>
        /// Do / Don't message.
        /// </summary>
        public string Message { get; set; }
            = string.Empty;

        /// <summary>
        /// True = Do
        /// False = Don't
        /// </summary>
        public bool IsDos { get; set; }

        /// <summary>
        /// Display order.
        /// </summary>
        public decimal SequenceNo { get; set; }

        public virtual MasterDosDontsDocument?
            Document
        { get; set; }
    }
}
