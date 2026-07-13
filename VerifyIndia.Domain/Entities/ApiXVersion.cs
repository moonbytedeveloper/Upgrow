using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities
{
    public class ApiXVersion : BaseEntity
    {
        public string? ApiCategoryUUID { get; set; }
        public string ApiXCategoryUUID {get; set;}
        public string VersionNo {get; set;}
        public string ApiMethod {get; set;}
        public string ApiPath   {get; set;}
        public string Title     {get; set;}
        public string Description {get; set;}
        public bool IsRequestBodyRequired {get; set;}  
        public int SequenceNo { get; set; }
    }
}
