using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities.Registration;

namespace VerifyIndia.Domain.IRepositories.Registration
{
    public interface ICustomerOtpRepository
    {
        Task<CustomerOtp?> GetLatestAsync(
            string customerUuid);

        Task AddAsync(
            CustomerOtp otp);

        Task UpdateAsync(
            CustomerOtp otp);

        Task InvalidateExistingAsync(
            string customerUuid);
    }
}
