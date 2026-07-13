using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application;
using Upgrow.Application.Verification.Documents;

namespace Upgrow.Infrastructure.Repositories.CustomerPanel.TransactionDocuments
{
    public sealed class TemplateRepository
    : ITemplateRepository
    {
        private readonly AppDbContext _dbcontext;
        public TemplateRepository(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<string> GetTemplateAsync(
            string verificationCode,
            CancellationToken cancellationToken)
        {
            if (verificationCode == Constants.VerificationCodes.AadhaarVerifyOtp)
            {
                verificationCode = Constants.VerificationCodes.AadhaarSendOTP;
            }

            var api = await _dbcontext.Master_Api.Where(x => x.Code == verificationCode && x.IsActive == true).FirstOrDefaultAsync();

            if (api == null)
            {
                // throw exception
                throw new InvalidOperationException(
              $"No active API found for verification code '{verificationCode}'.");
            }

            return api.VerificationDocument;  
        }
    }
}
