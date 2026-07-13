using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Verification;
using VerifyIndia.Application.Verification.Interfaces;

namespace VerifyIndia.Infrastructure.Verification
{
    public class ApiEndpointRepository : IApiEndpointRepository
    {
        private readonly AppDbContext _context;

        public ApiEndpointRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ApiExecutionStep>> GetExecutionStepsAsync(
            string apiUuid)
        {
            return await _context.Api_Endpoint
                .Where(x =>
                    x.ApiUUID == apiUuid &&
                    x.IsActive)
                .OrderBy(x =>
                    x.Sequence)
                .Select(x =>
                    new ApiExecutionStep
                    {
                        VerificationCode =
                            x.VerificationCode!,

                        Sequence =
                            x.Sequence,

                        EndpointUrl =
                            x.EndpointUrl!,

                        HttpMethod =
                            x.HttpMethod!
                    })
                .ToListAsync();
        }
    }
}
