using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.WL;
using Upgrow.Application.DTO.DropDown;
using Upgrow.Application.DTO.WL;
using Upgrow.Application.DTOs;
using Upgrow.Application.IServices.Master;
using Upgrow.Domain.Common;
using Upgrow.Domain.Entities;

namespace Upgrow.Application.IServices.WL
{
    public interface IWLTestimonialService : IMasterService<WLTestimonialDto, WLTestimonialCommand> ,
        IWLBaseService<WL_MasterTestimonial, WLTestimonialDto, WLTestimonialCommand>
    {
    }
}
