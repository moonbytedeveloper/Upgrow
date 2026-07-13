using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Api;

namespace VerifyIndia.Application.Interfaces
{
    public interface IManageApiRepository
    {
        Task SaveApiConfigurationAsync(
            SaveApiConfigurationCommand command,
            string userUuid,
            string ipAddress);
    }
}
