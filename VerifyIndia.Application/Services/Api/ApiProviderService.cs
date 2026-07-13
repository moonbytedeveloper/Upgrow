using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public class ApiProviderService : MasterServiceBase<Api_Provider, ApiProviderDto, ApiProviderCommand>, IApiProviderService
    {
        public ApiProviderService(IMasterRepository<Api_Provider> repository, IMapper mapper)
            : base(repository, mapper) { }


        // Define which field to check for duplicates
        protected override async Task<bool> IsDuplicateAsync(ApiProviderCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.ProviderName!.ToLower().Trim() == command.ProviderName.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        // Define which fields to search
        protected override Expression<Func<Api_Provider, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.ProviderName != null && x.ProviderName.ToLower().Contains(searchTerm));
        }

        // Define sorting (optional - remove if default Id sorting is fine)
        protected override Func<IQueryable<Api_Provider>, IOrderedQueryable<Api_Provider>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "name" => q => isAsc ? q.OrderBy(x => x.ProviderName) : q.OrderByDescending(x => x.ProviderName),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }

        protected override async Task CreateAsync(ApiProviderCommand command, string userUuid, string ip, bool saveChanges = true)
        {
            command.Code = GenerateCodeFromName(command.ProviderName);
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

        public async Task<List<ApiProviderDto>> GetAllActiveAsync()
        {
            var entities = await _repository.GetAllActiveAsync();
            return _mapper.Map<List<ApiProviderDto>>(entities);
        }
        
    }
}
   
    
