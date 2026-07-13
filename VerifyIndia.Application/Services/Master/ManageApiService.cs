using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Api;
using VerifyIndia.Application.Interfaces;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Domain.IRepositories.Master;

namespace VerifyIndia.Application.Services.Master
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
