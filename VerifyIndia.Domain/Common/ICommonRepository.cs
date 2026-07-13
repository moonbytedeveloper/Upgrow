using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Common
{
    public interface ICommonRepository
    {
        Task<string?> GetPlatformOwnerUUID();

        Task<int> ChangeCurrentStep(string mobileNumber, string code);
    }
}
