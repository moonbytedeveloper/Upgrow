using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;

namespace Upgrow.Application.Commands.Api
{
    public class ApiXHeaderCommand : IMasterCommand
    {
      public string? UUID { get; set; }
      public string?  ApiXVersionUUID {get; set;}
      public string?  FieldName       {get; set;}
      public bool  IsRequired      {get; set;}
      public string?  DataType        {get; set;}
      public string? FieldDetails { get; set; }
    }
}
