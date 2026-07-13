using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.IRepositories
{
    public interface IAppSettingRepository
    {
        Task<string?> GetValueAsync(string key);
    }
}
