 
using VerifyIndia.Domain.Entities;

public interface IVerificationRequestLogRepository
{
    Task AddAsync(VerificationRequestLog log);
    Task<byte[]?> GetLastCurrentHashAsync();
}