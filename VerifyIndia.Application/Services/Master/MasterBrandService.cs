using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.Master
{
    public class MasterBrandService : MasterServiceBase<Master_Brand, MasterBrandDto, MasterBrandCommand>, IMasterBrandService
    {
        public MasterBrandService(IMasterRepository<Master_Brand> repository, IMapper mapper) : base(repository, mapper) { }

        protected override Expression<Func<Master_Brand, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            return x =>
            (x.Title != null && x.Title.ToLower().Contains(searchTerm));
        }

        protected override async Task<bool> IsDuplicateAsync(MasterBrandCommand command)
        {
            return await _repository.ExistsAsync(x => x.Title!.ToLower().Trim() == command.Title.ToLower().Trim() &&
            x.UUID != command.UUID);
        }
        protected override Func<IQueryable<Master_Brand>, IOrderedQueryable<Master_Brand>>? BuildSortExpression(
           string? sortColumn,
           string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "title" => q => isAsc ? q.OrderBy(x => x.Title) : q.OrderByDescending(x => x.Title),
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
