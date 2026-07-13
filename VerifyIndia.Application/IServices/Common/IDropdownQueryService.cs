using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Common.Dropdowns;

namespace VerifyIndia.Application.IServices.Common
{
    public interface IDropdownQueryService
    {
        Task<List<DropdownItemDto>> GetAsync(DropdownRequest request);
    }
}
