using System.Text.Json;
using Upgrow.Application.DTO.Auth;
using Upgrow.Application.IServices.Auth;
using Upgrow.Application.Utilities;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.Services.Auth
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