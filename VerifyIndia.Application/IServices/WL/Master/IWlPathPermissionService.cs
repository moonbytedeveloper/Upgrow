using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.IServices.WL.Master
{
    public interface IWlPathPermissionService
    {
        /// <summary>
        /// Check if a role has access to a specific path
        /// </summary>
        /// <param name="roleUuid">The role UUID to check</param>
        /// <param name="path">The path to check access for</param>
        /// <returns>True if role has access, false otherwise</returns>
        Task<bool> HasAccessToPathAsync(string roleUuid, string path);
    }
}
