using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.ActionLogs
{
    public class ActionLogDto
    {
        public decimal Id { get; set; }
        public string? EntityName { get; set; }
        public string? EntityUUID { get; set; }
        public string? RootEntityName { get; set; }
        public string? RootEntityUUID { get; set; }
        public string? ActionType { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public string? UserUUID { get; set; }
        public string? UserName { get; set; }
        public string? IPAddress { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
 
    }
}
