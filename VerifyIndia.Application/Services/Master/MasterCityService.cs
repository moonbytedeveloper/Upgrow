using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.Common.Dropdowns;
using VerifyIndia.Application.DTO.DropDown;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.DTOs;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.Master
{
    public class MasterCityService : MasterServiceBase<Master_City, MasterCityDto, MasterCityCommand>, IMasterCityService
    {
        private readonly IMasterRepository<Master_Country> _countryRepository;
        private readonly IMasterRepository<Master_State> _stateRepository;
        public MasterCityService(IMasterRepository<Master_City> repository, IMapper mapper,
             IMasterRepository<Master_Country> countryRepository,
             IMasterRepository<Master_State> stateRepository)
          : base(repository, mapper)
        {
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
        }       

        // Define which field to check for duplicates
        protected override async Task<bool> IsDuplicateAsync(MasterCityCommand command)
        {
            var title = command.Title?.ToLower().Trim() ?? string.Empty;
            var country = command.CountryUUID ?? string.Empty;
            var state = command.StateUUID ?? string.Empty;

            //return await _repository.ExistsAsync(x =>
            //    x.Title!.ToLower().Trim() == command.Title.ToLower().Trim() &&
            //    x.UUID != command.UUID);
            return await _repository.ExistsAsync(x =>
               x.Title != null && x.Title.ToLower().Trim() == title &&
               (x.CountryUUID ?? string.Empty) == country &&
               (x.StateUUID ?? string.Empty) == state &&
               x.UUID != command.UUID);
        }

        // Define which fields to search
        protected override Expression<Func<Master_City, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.Title != null && x.Title.ToLower().Contains(searchTerm)) ||
                (x.ShortTitle != null && x.ShortTitle.ToLower().Contains(searchTerm));
        }

        // Define sorting (optional - remove if default Id sorting is fine)
        protected override Func<IQueryable<Master_City>, IOrderedQueryable<Master_City>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "name" => q => isAsc ? q.OrderBy(x => x.Title) : q.OrderByDescending(x => x.Title),
                "shortname" => q => isAsc ? q.OrderBy(x => x.ShortTitle) : q.OrderByDescending(x => x.ShortTitle),
                "country" => q => isAsc ? q.OrderBy(x => x.CountryUUID) : q.OrderByDescending(x => x.CountryUUID),
                "state" => q => isAsc ? q.OrderBy(x => x.StateUUID) : q.OrderByDescending(x => x.StateUUID),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }
        public async Task<List<MasterDropDownDto>> GetCityDropDownWithFK()
        {             
            var entities = await _repository.GetAllActiveAsync();

            return entities                
                .Select(c => new MasterDropDownDto
                {
                    ForeignKey = c.StateUUID,
                    UUID = c.UUID!,
                    Title = c.Title  
                })
                .OrderBy(e => e.Title)
                .ToList();
       
        }

        /*public async Task<List<DropdownItemDto>> GetDropdownByStateAsync(string stateUuid)
        {
            var cities =
                await _repository.FindAllAsync(
                    x => x.StateUUID == stateUuid
                         && x.IsActive);

            return cities
                .OrderBy(x => x.Title)
                .Select(x => new DropdownItemDto
                {
                    Text = x.UUID!,
                    Value = x.Title!
                })
                .ToList();
        }*/


        public async Task<List<DropdownItemDto>>
            GetDropdownByStateAsync(
                string stateUuid)
        {
            var cities =
                await _repository.GetAllActiveAsync();

            return cities
                .Where(x =>
                    x.StateUUID == stateUuid)
                .OrderBy(x => x.Title)
                .Select(x => new DropdownItemDto
                {
                    Text = x.Title!,
                    Value = x.UUID!
                })
                .ToList();
        }

    }
}

