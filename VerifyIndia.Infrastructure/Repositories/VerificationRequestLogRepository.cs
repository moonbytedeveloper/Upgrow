using Microsoft.EntityFrameworkCore;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;
using VerifyIndia.Infrastructure;

public class VerificationRequestLogRepository : IVerificationRequestLogRepository
{
    private readonly AppDbContext _context;

    public VerificationRequestLogRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(VerificationRequestLog log)
    {
        await _context.VerificationRequestLogs.AddAsync(log);
        await _context.SaveChangesAsync();
    }

    public async Task<byte[]?> GetLastCurrentHashAsync()
    {
        return await _context.VerificationRequestLogs
            .AsNoTracking()
            .OrderByDescending(x => x.Id)
            .Select(x => x.CurrentHash)
            .FirstOrDefaultAsync();
    }
}