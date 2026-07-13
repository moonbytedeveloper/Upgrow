using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.WL;
using Upgrow.Application.DTO.WL;
using Upgrow.Application.IServices.Master;
using Upgrow.Domain.Entities;

namespace Upgrow.Application.IServices.WL
{
    public interface IWLClientsService : IMasterService<WLClientsDto, WLClientsCommand>,
          IWLBaseService<WL_Clients, WLClientsDto, WLClientsCommand>
    {
    }
}
