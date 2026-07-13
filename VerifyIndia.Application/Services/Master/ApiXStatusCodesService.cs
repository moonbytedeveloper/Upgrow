using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public class ApiXStatusCodesService : MasterServiceBase<ApiXStatusCodes, ApiXStatusCodesDto, ApiXStatusCodesCommand>, IApiXStatusCodesService
    {
        public ApiXStatusCodesService(IMasterRepository<ApiXStatusCodes> repository, IMapper mapper)
         : base(repository, mapper) { }

        // Duplicate check: Title must be unique (case-insensitive, trimmed)
        protected override async Task<bool> IsDuplicateAsync(ApiXStatusCodesCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.Title.ToLower().Trim() == command.Title.ToLower().Trim() &&
                x.UUID != command.UUID);
        }


        protected override Expression<Func<ApiXStatusCodes, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.Title != null && x.Title.ToLower().Contains(searchTerm)) ||
                (x.StatusCode.ToString().Contains(searchTerm));
        }


        protected override Func<IQueryable<ApiXStatusCodes>, IOrderedQueryable<ApiXStatusCodes>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "title" => q => isAsc ? q.OrderBy(x => x.Title) : q.OrderByDescending(x => x.Title),
                "statuscode" => q => isAsc ? q.OrderBy(x => x.StatusCode) : q.OrderByDescending(x => x.StatusCode),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }
    }
}