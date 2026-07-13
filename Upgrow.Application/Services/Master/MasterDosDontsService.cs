using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.IServices.Master;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.Services.Master
{
    public class MasterDosDontsService : MasterServiceBase<Master_DosDonts, MasterDosDontsDto, MasterDosDontsCommand>, IMasterDosDontsService
    {
        public MasterDosDontsService(IMasterRepository<Master_DosDonts> repository, IMapper mapper)
            : base(repository, mapper) { }

        public async Task<List<GetDosDontsDto>> GetDosDontsListAsync(string documentUUID, bool isDos)
        {
            var entities = await _repository.GetAllActiveAsync();

            return entities
                .Where(x => x.DocumentUUID == documentUUID && x.IsDos == isDos && x.IsActive)
                .OrderBy(x => x.SequenceNo)
                .Select(x => new GetDosDontsDto
                {
                    Message = x.Message,                   
                    SequenceNo = x.SequenceNo
                })
                .ToList();
        }

        // Define which field to check for duplicates
        protected override async Task<bool> IsDuplicateAsync(MasterDosDontsCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.Message!.ToLower().Trim() == command.Message.ToLower().Trim() &&
                x.IsDos == command.IsDos &&
                x.SequenceNo == command.SequenceNo &&
                x.UUID != command.UUID);
        }

        // Define which fields to search
        protected override Expression<Func<Master_DosDonts, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.Message != null && x.Message.ToLower().Contains(searchTerm));
        }

        // Define sorting
        protected override Func<IQueryable<Master_DosDonts>, IOrderedQueryable<Master_DosDonts>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "message" => q => isAsc ? q.OrderBy(x => x.Message) : q.OrderByDescending(x => x.Message),
                "isdos" => q => isAsc ? q.OrderBy(x => x.IsDos) : q.OrderByDescending(x => x.IsDos),
                "sequenceno" => q => isAsc ? q.OrderBy(x => x.SequenceNo) : q.OrderByDescending(x => x.SequenceNo),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }
    }
}