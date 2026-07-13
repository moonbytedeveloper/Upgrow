using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Api;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.DTO.Api;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.IServices.Api;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Application.Services.Master;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.Api
{
    public class ApiComponentsService : MasterServiceBase<Api_Components, ApiComponentsDto, ApiComponentsCommand>, IApiComponentsService
    {
        public ApiComponentsService(IMasterRepository<Api_Components> repository, IMapper mapper)
           : base(repository, mapper) { }

        // Define which field to check for duplicates
        protected override async Task<bool> IsDuplicateAsync(ApiComponentsCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.Name!.ToLower().Trim() == command.Name.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        // Define which fields to search
        protected override Expression<Func<Api_Components, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.Name != null && x.Name.ToLower().Contains(searchTerm)) ||
                (x.Code != null && x.Code.ToLower().Contains(searchTerm));
        }

        // Define sorting (optional - remove if default Id sorting is fine)
        protected override Func<IQueryable<Api_Components>, IOrderedQueryable<Api_Components>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "name" => q => isAsc ? q.OrderBy(x => x.Name) : q.OrderByDescending(x => x.Name),
                "code" => q => isAsc ? q.OrderBy(x => x.Code) : q.OrderByDescending(x => x.Code),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }
        protected override async Task CreateAsync(ApiComponentsCommand command, string userUuid, string ip, bool saveChanges = true)
        {
            command.Code = GenerateCodeFromName(command.Name);
            await base.CreateAsync(command, userUuid, ip);
        }

        private static string GenerateCodeFromName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return string.Empty;

            // Trim, replace any whitespace sequence with single underscore, uppercase
            var normalized = Regex.Replace(name.Trim(), @"\s+", "_").ToUpperInvariant();

            // Remove any characters except A-Z, 0-9 and underscore to keep code clean
            normalized = Regex.Replace(normalized, @"[^A-Z0-9_]", string.Empty);

            return normalized;
        }

    }
}
