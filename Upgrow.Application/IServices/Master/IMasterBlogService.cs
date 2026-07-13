using Upgrow.Application.DTOs.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.IServices.Master;

namespace Upgrow.Application.IServices.Master
{
    public interface IMasterBlogService : IMasterService<MasterBlogDto, MasterBlogCommand>
    {
        // Add any Blog-specific methods here if needed
    }
}
