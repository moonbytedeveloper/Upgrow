using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.IServices;
using VerifyIndia.Domain.Common;

namespace VerifyIndia.Application.Services
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
