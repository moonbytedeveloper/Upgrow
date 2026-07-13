using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Credential;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.DTO.Credential;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.IServices.Master;

namespace VerifyIndia.Application.IServices.Credential
{
    public interface IPaymentGatewayCredentialService : IMasterService<PaymentGatewayCredentialDto, PaymentGatewayCredentialCommand>
    {

    }
}
