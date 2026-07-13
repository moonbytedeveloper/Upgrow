using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public class WLMasterCountryService : MasterServiceBase<WL_MasterCountry, WLMasterCountryDto, WLMasterCountryCommand>, IWLMasterCountryService
    {
        public WLMasterCountryService(IMasterRepository<WL_MasterCountry> repository, IMapper mapper)
         : base(repository, mapper) { }

        // Define which field to check for duplicates
        protected override async Task<bool> IsDuplicateAsync(WLMasterCountryCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.Title!.ToLower().Trim() == command.Title.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        // Define which fields to search
        protected override Expression<Func<WL_MasterCountry, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.Title != null && x.Title.ToLower().Contains(searchTerm)) ||
                (x.ShortTitle != null && x.ShortTitle.ToLower().Contains(searchTerm));
        }

        // Define sorting (optional - remove if default Id sorting is fine)
        protected override Func<IQueryable<WL_MasterCountry>, IOrderedQueryable<WL_MasterCountry>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "name" => q => isAsc ? q.OrderBy(x => x.Title) : q.OrderByDescending(x => x.Title),
                "shortname" => q => isAsc ? q.OrderBy(x => x.ShortTitle) : q.OrderByDescending(x => x.ShortTitle),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }

        protected override Task ValidateAsync(WLMasterCountryCommand command)
        {
            var validationRules = new List<(Func<bool> condition, string errorMessage)>
            {
                (
                    () => !string.IsNullOrWhiteSpace(command.ShortTitle) && command.ShortTitle.Length > command.Title.Length,
                    "Short Title cannot be longer than Title."
                ),
                (
                    () => !string.IsNullOrWhiteSpace(command.ShortTitle) &&
                          !System.Text.RegularExpressions.Regex.IsMatch(command.ShortTitle, @"^[A-Za-z\s]+$"),
                    "Short Title can only contain letters and spaces."
                ),
                (
                    () => command.Title.Trim().Length < 2,
                    "Title must be at least 2 characters long."
                ),
                (
                    () => command.Title.Length > 100,
                    "Title cannot exceed 100 characters."
                ),
                (
                    () => !string.IsNullOrWhiteSpace(command.ShortTitle) && command.ShortTitle.Length > 50,
                    "Short Title cannot exceed 50 characters."
                )
            };

            var errors = validationRules
                .Where(rule => rule.condition())
                .Select(rule => rule.errorMessage)
                .ToList();

            if (errors.Any())
            {
                throw new ValidationException(string.Join(" ", errors));
            }

            return Task.CompletedTask;
        }
    }
}



