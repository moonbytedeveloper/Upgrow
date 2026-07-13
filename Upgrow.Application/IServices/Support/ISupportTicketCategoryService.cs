using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.Commands.Support;
using Upgrow.Application.DTO.DropDown;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.DTO.Support;
using Upgrow.Application.DTO.WL;
using Upgrow.Application.IServices.Master;
using Upgrow.Domain.Entities;

namespace Upgrow.Application.IServices.Support
{
    public interface ISupportTicketCategoryService : IMasterService<SupportTicketCategoryDto, SupportTicketCategoryCommand>
    {
        Task<List<SupportTicketCategoryDto>> GetAllAsync(Expression<Func<Support_TicketCategory, bool>> predicate);
    }
}
