using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Master
{
    public class MasterDocumentDto
    {
        public string UUID { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string FileType { get; set; } = null!;
        public string Path { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
