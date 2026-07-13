using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.WL;
using VerifyIndia.Domain.Entities.WL.Master;

namespace VerifyIndia.Application.IServices.WL.Master
{
    public interface IWLActivityLogsLogsService
    {
        /// <summary>
        /// Add activity log with automatic hash computation, digital signature, and payload serialization
        /// Handles: PreviousHash (from last entry), CurrentHash (new SHA256), DigitalSignature (RSA), PayLoad (JSON)
        /// </summary>
        Task AddAsync(WL_ActivityLogs log, WLActivityLogDto dto);
    }
}
