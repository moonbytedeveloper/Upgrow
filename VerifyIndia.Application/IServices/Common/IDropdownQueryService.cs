using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Common.Dropdowns;

namespace Upgrow.Application.IServices.Common
{
    public interface IDropdownQueryService
    {
        Task<List<DropdownItemDto>> GetAsync(DropdownRequest request);
    }
}
