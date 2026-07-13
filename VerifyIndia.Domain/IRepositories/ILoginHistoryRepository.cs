using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.IRepositories
{
    public interface ILoginHistoryRepository
    {
        Task<DateTime?> GetLastLoginAsync(string userUuid);
        Task RecordLoginAsync(string userUuid, string ipAddress, DateTime loginAt);
    }
}
