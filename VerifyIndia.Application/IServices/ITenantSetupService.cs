using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.IServices
{
    public interface ITenantSetupService
    {
        int TenantId { get; }
        string Identifier { get; }
    }
}
