using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.WL.Master;
using Upgrow.Application.DTO.WL.Master;
using Upgrow.Application.IServices.Master;

namespace Upgrow.Application.IServices.WL.Master
{
    public interface IWLWhatsappCredentialService : IMasterService<WLWhatsappCredentialDto, WLWhatsappCredentialCommand>
    {
    }
}
