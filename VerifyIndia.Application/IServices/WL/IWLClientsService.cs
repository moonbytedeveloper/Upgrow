using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.WL;
using VerifyIndia.Application.DTO.WL;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Application.IServices.WL
{
    public interface IWLClientsService : IMasterService<WLClientsDto, WLClientsCommand>,
          IWLBaseService<WL_Clients, WLClientsDto, WLClientsCommand>
    {
    }
}
