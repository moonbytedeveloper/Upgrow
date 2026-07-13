using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.WL
{
    /// <summary>
    /// DTO for WL Activity Log API serialization
    /// </summary>
    public sealed class WLActivityLogDto
    {        
        public string? UserUUID { get; set; }
        public string? UserName { get; set; }  // Add this
        public string? ActivityType { get; set; }
        public string? Description { get; set; }
        public string? MenuName { get; set; }
        public string? PageUrl { get; set; }
        public string? IPAddress { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
