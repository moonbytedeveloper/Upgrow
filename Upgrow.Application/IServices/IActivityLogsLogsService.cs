using Upgrow.Domain.Entities;

namespace Upgrow.Application.IServices
{
    public interface IActivityLogsLogsService
    {
        Task AddAsync(ActivityLogs log);
    }
}