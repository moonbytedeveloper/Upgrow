using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.IServices;
using Upgrow.Domain.Common;

namespace Upgrow.Application.Services
{
    public class CommonService : ICommonService
    {
        private readonly ICommonRepository _repo;

        public CommonService(ICommonRepository repo)
        {
            _repo = repo;
        }

        public Task<string?> GetPlatformOwnerUUID()
        {
            return _repo.GetPlatformOwnerUUID();
        }
    }
}
