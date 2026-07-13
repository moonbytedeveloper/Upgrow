using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Upgrow.Application.DTO.Auth;
using Upgrow.Application.Utilities;
using Upgrow.Application.IServices.Auth;

using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.Services.Auth
{
    public class LoginAttemptLogsService : ILoginAttemptLogsService
    {
        private readonly ILoginAttemptRepository _repository;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;

        public LoginAttemptLogsService(ILoginAttemptRepository repository, IConfiguration config, IWebHostEnvironment env)
        {
            _repository = repository;
            _config = config;
            _env = env;
        }

        public async Task AddAsync(LoginAttemptDto dto)
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
                // keep behavior consistent with action logging (signature optional if key fails)
            }

            var attempt = new LoginAttempts
            {
                Payload = json,
                PreviousHash = previousHash,
                CurrentHash = currentHash,
                DigitalSignature = signature
            };

            await _repository.AddAsync(attempt);
        }
    }
}