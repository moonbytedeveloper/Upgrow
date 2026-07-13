using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Api;
using Upgrow.Application.DTO.Master;

namespace Upgrow.Application.IServices.Master
{
    public interface IManageApiService
    {
        Task SaveApiConfigurationAsync(
            SaveApiConfigurationCommand command,
            string userUuid,
            string ipAddress);
    }
}
