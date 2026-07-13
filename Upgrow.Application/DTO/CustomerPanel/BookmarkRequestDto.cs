using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.CustomerPanel
{
    public class BookmarkRequestDto
    {
        public string NewsUUID { get; set; } = string.Empty;
        public bool IsBookMarked = true;
    }
}

