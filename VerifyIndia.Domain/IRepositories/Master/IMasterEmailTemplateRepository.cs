using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Domain.IRepositories.Master
{
    public interface IMasterEmailTemplateRepository
    {
        Task<Master_EmailTemplate?> GetByIdAsync(decimal id);
    }
}
