using Upgrow.Application.Utilities;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;

public class VerificationRequestLogService : IVerificationRequestLogService
{
    private readonly IVerificationRequestLogRepository _repository;

    public VerificationRequestLogService(IVerificationRequestLogRepository repository)
    {
        _repository = repository;
    }

    public async Task LogAsync(string payload)
    {
        try
        {
            var prevHash = await _repository.GetLastCurrentHashAsync();

            var currentHash = ActionLogHashUtility.ComputeCurrentHash(payload);
            byte[]? signature = null;
            try
            {
                var privateKeyPath = KeyPathResolver.GetPrivateKeyPath();
                signature = ActionLogHashUtility.ComputeDigitalSignature(currentHash, privateKeyPath);
            }
            catch { }

            var log = new VerificationRequestLog
            {
                Payload = payload,
                PreviousHash = prevHash,
                CurrentHash = currentHash,
                DigitalSignature = signature
            };

            await _repository.AddAsync(log);

        } 
        catch (Exception ex)
        {

        }
        
    }
}