using Upgrow.Application.DTOs.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;

namespace Upgrow.Application.IServices.Master
{
    public interface IMasterBlogCategoryService : IMasterService<MasterBlogCategoryDto, MasterBlogCategoryCommand>
    {
    }
}
