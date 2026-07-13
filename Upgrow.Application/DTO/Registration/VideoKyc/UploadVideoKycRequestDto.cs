using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Registration.VideoKyc
{
    public sealed class UploadVideoKycRequestDto
    {
        public string VideoUrl { get; set; } = string.Empty;

    }
}
