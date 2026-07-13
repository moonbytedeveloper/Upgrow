using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Website;
using VerifyIndia.Application.DTO.Website;
using VerifyIndia.Application.IServices.Master;

namespace VerifyIndia.Application.IServices.Website
{
    public interface IKnowledgeHubCategoryService : IMasterService<KnowledgeHubCategoryDto, KnowledgeHubCategoryCommand>
    {
        Task<List<KnowledgeHubCategoryDto>> GetAllAsync();
    }
}
