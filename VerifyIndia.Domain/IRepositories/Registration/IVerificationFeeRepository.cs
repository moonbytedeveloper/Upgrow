using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities.Registration;

namespace VerifyIndia.Domain.IRepositories.Registration
{
    public interface IVerificationFeeRepository
    {
        Task<Master_VerificationFee?>
            GetByTypeAsync(
                string verificationType);
    }
}
