using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.Common.Dropdowns;
using VerifyIndia.Application.DTO.DropDown;
using VerifyIndia.Application.DTO.Master;

namespace VerifyIndia.Application.IServices.Master
{
    public interface IMasterCityService : IMasterService<MasterCityDto, MasterCityCommand>
    {
        Task<List<MasterDropDownDto>> GetCityDropDownWithFK();

        Task<List<DropdownItemDto>> GetDropdownByStateAsync(string stateUuid);
    }
}
