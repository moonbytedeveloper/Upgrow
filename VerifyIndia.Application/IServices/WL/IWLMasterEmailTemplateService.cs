using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.Commands.WL;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.DTO.WL;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Domain.Entities.WL;

namespace VerifyIndia.Application.IServices.WL
{
    public interface IWLMasterEmailTemplateService
        : IMasterService<WLMasterEmailTemplateDto, WLMasterEmailTemplateCommand>,
          IWLBaseService<WL_MasterEmailTemplate, WLMasterEmailTemplateDto, WLMasterEmailTemplateCommand>
    {
        Task<WLMasterEmailTemplateDto?> GetByIdAsync(decimal templateId);
    }
}
