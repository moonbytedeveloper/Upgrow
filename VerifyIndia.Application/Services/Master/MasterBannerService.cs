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
    public class MasterBannerService : MasterServiceBase<Master_Banner, MasterBannerDto, MasterBannerCommand>, IMasterBannerService
    {
        public MasterBannerService(IMasterRepository<Master_Banner> repository, IMapper mapper) : base(repository, mapper) { }

        protected override Expression<Func<Master_Banner, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            return x =>
            (x.MainTitle != null && x.MainTitle.ToLower().Contains(searchTerm)) ||
            (x.SubTitle != null && x.SubTitle.ToLower().Contains(searchTerm)) ||
            (x.ButtonText != null && x.ButtonText.ToLower().Contains(searchTerm))  ||
            (x.SequenceNo != null && Convert.ToString(x.SequenceNo).ToLower().Contains(searchTerm)) ||
            (x.ButtonURL != null && x.ButtonURL.ToLower().Contains(searchTerm));
        }

        protected override async Task<bool> IsDuplicateAsync(MasterBannerCommand command)
        {
            return await _repository.ExistsAsync(x => x.MainTitle!.ToLower().Trim() == command.MainTitle.ToLower().Trim() &&
            x.UUID != command.UUID);
        }
        protected override Func<IQueryable<Master_Banner>, IOrderedQueryable<Master_Banner>>? BuildSortExpression(
           string? sortColumn,
           string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "maintitle" => q => isAsc ? q.OrderBy(x => x.MainTitle) : q.OrderByDescending(x => x.MainTitle),
                "subtitle" => q => isAsc ? q.OrderBy(x => x.SubTitle) : q.OrderByDescending(x => x.SubTitle),
                "buttontext" => q => isAsc ? q.OrderBy(x => x.ButtonText) : q.OrderByDescending(x => x.ButtonText),
                "buttonurl" => q => isAsc ? q.OrderBy(x => x.ButtonURL) : q.OrderByDescending(x => x.ButtonURL),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }

        public override async Task DeleteAsync(string uuid, string userUuid, string ip)
        {
            var entity = await _repository.GetByUuidAsync(uuid)
                ?? throw new Exception("Banner not found");

            entity.IsActive = false;

            await _repository.UpdateAsync(entity);
        }
    }
}
