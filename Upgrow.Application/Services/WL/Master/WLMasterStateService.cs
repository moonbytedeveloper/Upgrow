
using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.DTO.DropDown;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.DTOs;
using Upgrow.Application.IServices.Master;
using Upgrow.Application.Services.Master;
using Upgrow.Domain.Common;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.Services
{
    public class WLMasterStateService : MasterServiceBase<WL_MasterState, WLMasterStateDto, WLMasterStateCommand>, IWLMasterStateService
    {
        public WLMasterStateService(IMasterRepository<WL_MasterState> repository, IMapper mapper)
           : base(repository, mapper)
        {
            
        }


    

        public async Task<List<MasterDropDownDto>> GetDropdownByCountryAsync(string countryUuid)
        {
            if (string.IsNullOrWhiteSpace(countryUuid))
                return new List<MasterDropDownDto>();

           // Get all active states and map to command DTO to access CountryUUID
            var entities = await _repository.GetAllActiveAsync();

            var commands = entities
                .Select(e => _mapper.Map<WLMasterStateCommand>(e))
                .Where(c => !string.IsNullOrEmpty(c.CountryUUID) && c.CountryUUID == countryUuid)
                .Select(c => new MasterDropDownDto
                {
                    UUID = c.UUID!,
                    Title = c.Title ?? string.Empty
                })
                .OrderBy(x => x.Title)
                .ToList();

            return commands;
        }

        // Define which field to check for duplicates
        protected override async Task<bool> IsDuplicateAsync(WLMasterStateCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.Title!.ToLower().Trim() == command.Title.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        // Define which fields to search
        // ... existing code ...

        // Define which fields to search
        protected override Expression<Func<WL_MasterState, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.Title != null && x.Title.ToLower().Contains(searchTerm)) ||
                (x.ShortTitle != null && x.ShortTitle.ToLower().Contains(searchTerm)) ||
                (x.CountryUUID != null && x.CountryUUID.ToLower().Contains(searchTerm));  // Added search on country name
        }

        // ... existing code ...

        // Define sorting (optional - remove if default Id sorting is fine)
        protected override Func<IQueryable<WL_MasterState>, IOrderedQueryable<WL_MasterState>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "name" => q => isAsc ? q.OrderBy(x => x.Title) : q.OrderByDescending(x => x.Title),
                "countryname" => q => isAsc ? q.OrderBy(x => x.CountryUUID) : q.OrderByDescending(x => x.CountryUUID),
                "shortname" => q => isAsc ? q.OrderBy(x => x.ShortTitle) : q.OrderByDescending(x => x.ShortTitle),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }

    }
}

