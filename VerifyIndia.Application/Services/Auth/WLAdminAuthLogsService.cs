using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using Upgrow.Application.IServices.Auth;
using Upgrow.Application.Utilities; // Assuming ActionLogHashUtility is here
using Upgrow.Domain.Entities.WL;
using Upgrow.Domain.IRepositories.LoginLogs;

namespace Upgrow.Application.Services.Auth
{
    public class WLAdminAuthLogsService : IWLAdminAuthLogsService
    {
        private readonly IWLAdminAuthLogsRepository _repository;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;

        public WLAdminAuthLogsService(IWLAdminAuthLogsRepository repository, IWebHostEnvironment env, IConfiguration config)
        {
            _repository = repository;
            _env = env;
            _config = config;
        }

        public async Task LogAuthActionActivityAsync(string payload)
        {
            var log = new WL_AdminAuthLogs
            {
                Payload = payload,
                PreviousHash = await _repository.GetLastCurrentHashAsync(),
                CurrentHash = ActionLogHashUtility.ComputeCurrentHash(payload)
            };

            var privateKeyPath = KeyPathResolver.GetPrivateKeyPath();
            try
            {
                log.DigitalSignature = ActionLogHashUtility.ComputeDigitalSignature(log.CurrentHash, privateKeyPath);
            }
            catch
            {
                // Silent catch matching your existing implementation. Log exception here if required.
            }

            await _repository.AddLogAsync(log);
        }
    }
}