using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories.Master;

namespace VerifyIndia.Infrastructure.Repositories.Master
{
    public class MasterDosDontsDocumentRepository : IMasterDosDontsDocumentRepository
    {
        private readonly AppDbContext _context;

        public MasterDosDontsDocumentRepository(
            AppDbContext context)
        {
            _context = context;
        }

        public async Task<MasterDosDontsDocument?>
    GetActiveAsync()
        {
            return await _context
                .MasterDosDontsDocument
                .FirstOrDefaultAsync(x =>
                    x.IsActive &&
                    x.Status == "Published");
        }
    }
}
