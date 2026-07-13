using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities.Registration;
using VerifyIndia.Domain.IRepositories.Registration;

namespace VerifyIndia.Infrastructure.Repositories.CustomerPanel.Registration
{
    public class VerificationFeeRepository
        : IVerificationFeeRepository
    {
        private readonly AppDbContext _context;

        public VerificationFeeRepository(
            AppDbContext context)
        {
            _context = context;
        }

        public async Task<Master_VerificationFee?> GetByTypeAsync(string verificationType)
        {
            return await _context
                .Master_VerificationFee
                .FirstOrDefaultAsync(
                    x =>
                        x.VerificationType ==
                        verificationType &&
                        x.IsActive);
        }
    }
}
