using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.Commands.WL;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.DTO.WL;
using Upgrow.Application.IServices.Master;
using Upgrow.Domain.Entities.WL;

namespace Upgrow.Application.IServices.WL
{
    public interface IWLMasterEmailTemplateService
        : IMasterService<WLMasterEmailTemplateDto, WLMasterEmailTemplateCommand>,
          IWLBaseService<WL_MasterEmailTemplate, WLMasterEmailTemplateDto, WLMasterEmailTemplateCommand>
    {
        Task<WLMasterEmailTemplateDto?> GetByIdAsync(decimal templateId);
    }
}
