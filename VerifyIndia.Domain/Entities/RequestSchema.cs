using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Domain.Entities
{
    public class RequestSchema : BaseEntity
    {
      public string  ApiXVersionUUID {get; set;}
      public string  RequestType     {get; set;}
      public string RequestJson { get; set; }
    }
}
