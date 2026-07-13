using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.IServices
{
    public interface IDomainResolverService
    {
       
        /// Resolve tenant identifier (company name) from the given host.
        
        Task<string?> ResolveCompanyIdentifierAsync(string host);
        
        //Task<string?> BuildAbsoluteUrl(string? path);

        string BuildAbsoluteUrl(string? path);
    }
}
