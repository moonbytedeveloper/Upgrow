using System.ComponentModel.DataAnnotations.Schema;
using Upgrow.Domain.Entities;

namespace UpgrowAdminPanel.Models.Apix
{
    public class ApiXHeaderVM
    {
        public int Id { get; set; }
       
        public int ApiXVersionId { get; set; }
        
        public ApiXVersion? ApiXVersion { get; set; }
       
        public string FieldName { get; set; } = string.Empty;

        public string DataType { get; set; } = string.Empty;

        // If header needs an ApiVersion separate from parent, keep it; otherwise remove
        public string? ApiVersion { get; set; }

        public bool IsRequestBodyRequired { get; set; }
    }
}
