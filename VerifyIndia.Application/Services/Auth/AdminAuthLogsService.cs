using System.Text.Json;
using VerifyIndia.Application.DTO.Auth;
using VerifyIndia.Application.IServices.Auth;
using VerifyIndia.Application.Utilities;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.Auth
{
    public class AdminAuthLogsService : IAdminAuthLogsService
    {
        private readonly IAdminAuthLogsRepository _repository;

        public AdminAuthLogsService(IAdminAuthLogsRepository repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(AdminAuthLogDto dto)
        {
            var json = JsonSerializer.Serialize(dto);

            var previousHash = await _repository.GetLastCurrentHashAsync();
            var currentHash = ActionLogHashUtility.ComputeCurrentHash(json);

            var privateKeyPath = KeyPathResolver.GetPrivateKeyPath();

            byte[]? signature = null;
            try
            {
                signature = ActionLogHashUtility.ComputeDigitalSignature(currentHash, privateKeyPath);
            }
            catch
            {
                // keep behavior consistent with AppDbContext audit logging (signature optional if key fails)
            }

            var entity = new AdminAuthLogs
            {
                Payload = json,
                PreviousHash = previousHash,
                CurrentHash = currentHash,
                DigitalSignature = signature
            };

            await _repository.AddAsync(entity);
        }
    }
}