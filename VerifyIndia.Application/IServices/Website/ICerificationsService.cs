using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Website;
using VerifyIndia.Application.DTO.Website;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Application.IServices.Website
{
    public interface ICerificationsService : IMasterService<CertificationsDto, CertificationsCommand>
    {
    }
}
