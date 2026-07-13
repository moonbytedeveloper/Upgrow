using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Domain.Entities.Registration
{
    public sealed class CustomerBusinessSession : BaseEntity
    {
        public string CustomerUUID
        {
            get;
            set;
        } = string.Empty;

        public string CustomerOrganizationUUID
        {
            get;
            set;
        } = string.Empty;

        public string RazorpayOrderId
        {
            get;
            set;
        } = string.Empty;

        public decimal Amount
        {
            get;
            set;
        }

        public DateTimeOffset CreatedAt
        {
            get;
            set;
        }
    }
}
