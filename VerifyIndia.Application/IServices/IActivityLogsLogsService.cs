using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Application.IServices
{
    public interface IActivityLogsLogsService
    {
        Task AddAsync(ActivityLogs log);
    }
}