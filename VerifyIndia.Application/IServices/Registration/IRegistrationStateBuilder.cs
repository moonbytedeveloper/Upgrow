using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.DTO.Registration;

namespace Upgrow.Application.IServices.Registration
{
    public interface IRegistrationStateBuilder
    {
        Task<RegistrationStateDto> BuildAsync(
            string customerUuid,
            bool generateKycChallenge = false);
    }
}
