using VerifyIndia.Application.DTOs.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.IServices.Master;

namespace VerifyIndia.Application.IServices.Master
{
    public interface IMasterBlogService : IMasterService<MasterBlogDto, MasterBlogCommand>
    {
        // Add any Blog-specific methods here if needed
    }
}
