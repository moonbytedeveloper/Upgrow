using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Api;
using VerifyIndia.Application.DTO.Api;
using VerifyIndia.Application.IServices.Api;
using VerifyIndia.Application.Services.Master;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.Api
{
    public class ApiInfoFieldsService : MasterServiceBase<ApiInfoFields, ApiInfoFieldsDto, ApiInfoFieldsCommand>, IApiInfoFieldsService

    {
        public ApiInfoFieldsService(IMasterRepository<ApiInfoFields> repository, IMapper mapper)
           : base(repository, mapper) { }

        public async Task<List<ApiInfoFieldsDto>> GetAllActiveAsync()
        {
            var entities = await _repository.GetAllActiveAsync();
            return _mapper.Map<List<ApiInfoFieldsDto>>(entities);
        }
 
        protected override async Task<bool> IsDuplicateAsync(ApiInfoFieldsCommand command)
        {
            return await _repository.ExistsAsync(x =>
                 x.Title!.ToLower().Trim() == command.Title.ToLower().Trim() &&
                 x.InfoSectionUUID!.ToLower().Trim() == command.InfoSectionUUID.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        protected override Expression<Func<ApiInfoFields, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                 (x.Title != null && x.Title.ToLower().Contains(searchTerm)) ||
                  (x.InfoSectionUUID != null && x.InfoSectionUUID.ToLower().Contains(searchTerm)) ||
                (x.UUID != null && x.UUID.ToLower().Contains(searchTerm));
        }

 
        protected override Func<IQueryable<ApiInfoFields>, IOrderedQueryable<ApiInfoFields>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "title" => q => isAsc ? q.OrderBy(x => x.Title) : q.OrderByDescending(x => x.Title),
                "sequence" => q => isAsc ? q.OrderBy(x => x.Sequence) : q.OrderByDescending(x => x.Sequence),
                "infosectionuuid" => q => isAsc ? q.OrderBy(x => x.InfoSectionUUID) : q.OrderByDescending(x => x.InfoSectionUUID),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }





    }
}

