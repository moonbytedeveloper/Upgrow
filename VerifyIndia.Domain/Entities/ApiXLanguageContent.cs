using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities
{
    public class ApiXLanguageContent : BaseEntity
    {
        public string? ApiXVersionUUID { get; set; }

        // FK to Master_ProgrammingLanguage.UUID
        public string? LanguageUUID { get; set; }

        // Optional denormalized language title (helpful for fast display)
        public string? LanguageContent { get; set; }

        // snippet content
        
    }
}