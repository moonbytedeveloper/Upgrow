using VerifyIndia.Domain.Common;

namespace VerifyIndia.Domain.Entities
{
    public class ApiXCodeMapper : BaseEntity
    {
        public string? ApiXVersionUUID { get; set; }
        public string? ResponseSchemaUUID { get; set; }
        public string? StatusUUID { get; set; }
        public string? CodeExampleUUID { get; set; }
        public string? ResponseJson { get; set; }
        public string? ShortDescription { get; set; }
        
    }
}
