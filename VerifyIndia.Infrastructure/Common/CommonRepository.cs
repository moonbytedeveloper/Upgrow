using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Common;

namespace VerifyIndia.Infrastructure.Common
{
    public class CommonRepository : ICommonRepository
    {
        private readonly AppDbContext _context;
        public CommonRepository(AppDbContext context) 
        {
            _context = context;
        }

        public async Task<string?> GetPlatformOwnerUUID()
        {
            return _context.Tenant.Where(x => x.IsActive && x.IsPlatformOwner && !string.IsNullOrEmpty(x.UUID)).Select(x => x.UUID).FirstOrDefault();
        }

        public async Task<int> ChangeCurrentStep(string mobileNumber, string code)
        {
            var customer = await _context.Master_Customer.Where(x => x.Mobile == mobileNumber).FirstOrDefaultAsync();

            customer.CurrentStep = code;
            _context.Update(customer);

            return await _context.SaveChangesAsync();
        }
    }
}
