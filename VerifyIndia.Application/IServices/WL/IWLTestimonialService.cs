using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.WL;
using VerifyIndia.Application.DTO.DropDown;
using VerifyIndia.Application.DTO.WL;
using VerifyIndia.Application.DTOs;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Application.IServices.WL
{
    public interface IWLTestimonialService : IMasterService<WLTestimonialDto, WLTestimonialCommand> ,
        IWLBaseService<WL_MasterTestimonial, WLTestimonialDto, WLTestimonialCommand>
    {
    }
}
