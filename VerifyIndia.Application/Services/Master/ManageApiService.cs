using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Api;
using Upgrow.Application.Interfaces;
using Upgrow.Application.IServices.Master;
using Upgrow.Domain.IRepositories.Master;

namespace Upgrow.Application.Services.Master
{
    public class ManageApiService : IManageApiService
    {
        private readonly IManageApiRepository _repository;

        public ManageApiService(
            IManageApiRepository repository)
        {
            _repository = repository;
        }

        public async Task SaveApiConfigurationAsync(
            SaveApiConfigurationCommand command,
            string userUuid,
            string ipAddress)
        {
            await _repository.SaveApiConfigurationAsync(
                command,
                userUuid,
                ipAddress);
        }
    }
}
