using VerifyIndia.Application.Utilities;
using VerifyIndia.Application.DTO;
using VerifyIndia.Application.IServices;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services
{
    public class ActivityLogsLogsService : IActivityLogsLogsService
    {
        private readonly IActivityLogsRepository _repository;

        public ActivityLogsLogsService(IActivityLogsRepository repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(ActivityLogs log)
        {
            // Use the Payload directly (already set as JSON by ActivityLogAttribute)
            var json = log.Payload;

            var logEntry = new ActivityLogs
            {
                Payload = json,
                PreviousHash = await _repository.GetLastCurrentHashAsync(),
                CurrentHash = ActionLogHashUtility.ComputeCurrentHash(json ?? string.Empty)  // Fallback to empty string if null
            };

            // Compute and assign digital signature
            var privateKeyPath = KeyPathResolver.GetPrivateKeyPath();
            try
            {
                logEntry.DigitalSignature = ActionLogHashUtility.ComputeDigitalSignature(logEntry.CurrentHash, privateKeyPath);
            }
            catch
            {

            }

            // Save to database
            await _repository.AddAsync(logEntry);
        }
    }
}