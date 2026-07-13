using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.Common.Dropdowns;
using Upgrow.Application.DTO.DropDown;
using Upgrow.Application.DTO.Master;

namespace Upgrow.Application.IServices.Master
{
    public interface IMasterCityService : IMasterService<MasterCityDto, MasterCityCommand>
    {
        Task<List<MasterDropDownDto>> GetCityDropDownWithFK();

        Task<List<DropdownItemDto>> GetDropdownByStateAsync(string stateUuid);
    }
}
