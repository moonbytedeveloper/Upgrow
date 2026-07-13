using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.Entities.WL;

namespace VerifyIndia.Domain.IRepositories.WL
{
    public interface IWLMasterEmailTemplateRepository
    {
        Task<WL_MasterEmailTemplate?> GetByIdAsync(decimal id);
    }
}
