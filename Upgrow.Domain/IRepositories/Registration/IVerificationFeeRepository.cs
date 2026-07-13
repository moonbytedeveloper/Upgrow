using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities.Registration;

namespace Upgrow.Domain.IRepositories.Registration
{
    public interface IVerificationFeeRepository
    {
        Task<Master_VerificationFee?>
            GetByTypeAsync(
                string verificationType);
    }
}
