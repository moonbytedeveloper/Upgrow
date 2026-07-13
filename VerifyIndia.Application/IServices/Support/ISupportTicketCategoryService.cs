using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.Commands.Support;
using VerifyIndia.Application.DTO.DropDown;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.DTO.Support;
using VerifyIndia.Application.DTO.WL;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Application.IServices.Support
{
    public interface ISupportTicketCategoryService : IMasterService<SupportTicketCategoryDto, SupportTicketCategoryCommand>
    {
        Task<List<SupportTicketCategoryDto>> GetAllAsync(Expression<Func<Support_TicketCategory, bool>> predicate);
    }
}
