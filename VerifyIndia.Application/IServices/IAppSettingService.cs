using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.IServices
{
    public interface IAppSettingService
    {
        Task<int> GetIntValueAsync(
            string key,
            int defaultValue);

        Task<string> GetValueAsync(
            string key,
            string defaultValue = "");  
    }
}
