using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.IServices
{
    public interface ICommonService
    {
        Task<string?> GetPlatformOwnerUUID();

    }
}
