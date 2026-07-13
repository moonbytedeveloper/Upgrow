using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities;
using Upgrow.Domain.Entities.WL;

namespace Upgrow.Domain.IRepositories.WL
{
    public interface IWLMasterEmailTemplateRepository
    {
        Task<WL_MasterEmailTemplate?> GetByIdAsync(decimal id);
    }
}
