using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Credential;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.DTO.Credential;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.IServices.Master;

namespace Upgrow.Application.IServices.Credential
{
    public interface IPaymentGatewayCredentialService : IMasterService<PaymentGatewayCredentialDto, PaymentGatewayCredentialCommand>
    {

    }
}
