using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities
{
    public class ResponseSchema : BaseEntity
    {        
        public string? ShortDescription    {get; set;}
        public bool IsSchemaForSuccess  {get; set;}
        public string? ApiXVersionUUID     {get; set;}
        public string? SchemaType          { get; set; }
    }
}
