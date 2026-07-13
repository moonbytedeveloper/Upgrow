using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Common
{
    public interface IMasterEntity
    {
        decimal Id { get; set; }
        string UUID { get; set; }
        bool IsActive { get; set; }
    }
}
