using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities
{
    public class ApiXHeaders : BaseEntity
    {
      public string  ApiXVersionUUID {get; set;}
      public string  FieldName       {get; set;}
      public bool  IsRequired      {get; set;}
      public string  DataType        {get; set;}
      public string FieldDetails { get; set; }
    }
}
