using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.Commands.WL.Master;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.DTO.WL.Master;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Application.IServices.WL.Master;
using VerifyIndia.Application.Services.Master;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.Entities.WL.Master;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.WL.Master
{
    public class WLMasterFinancialYearService : MasterServiceBase<WL_MasterFinancialYear, WLMasterFinancialYearDto, WLMasterFinancialYearCommand>, IWLFinancialYearService

    {
        public WLMasterFinancialYearService(IMasterRepository<WL_MasterFinancialYear> repository, IMapper mapper)
           : base(repository, mapper) { }

        // Define which field to check for duplicates
        protected override async Task<bool> IsDuplicateAsync(WLMasterFinancialYearCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.Title!.ToLower().Trim() == command.Title.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        // Define which fields to search
        protected override Expression<Func<WL_MasterFinancialYear, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.Title != null && x.Title.ToLower().Contains(searchTerm));
        }

        // Define sorting (optional - remove if default Id sorting is fine)
        protected override Func<IQueryable<WL_MasterFinancialYear>, IOrderedQueryable<WL_MasterFinancialYear>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "name" => q => isAsc ? q.OrderBy(x => x.Title) : q.OrderByDescending(x => x.Title),
                "startdate" => q => isAsc ? q.OrderBy(x => x.StartDate) : q.OrderByDescending(x => x.StartDate),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }

        protected override async Task ValidateAsync(WLMasterFinancialYearCommand command)
        {
            var errors = new List<string>();

            // Title validations
            if (string.IsNullOrWhiteSpace(command.Title))
            {
                errors.Add("Title is required.");
            }
            else
            {
                var title = command.Title.Trim();
                if (title.Length < 2)
                    errors.Add("Title must be at least 2 characters long.");
                if (title.Length > 100)
                    errors.Add("Title cannot exceed 100 characters.");
            }

            // Date presence
            if (!command.StartDate.HasValue)
                errors.Add("Start date is required.");
            if (!command.EndDate.HasValue)
                errors.Add("End date is required.");

            // If both dates present, validate ordering and overlaps
            if (command.StartDate.HasValue && command.EndDate.HasValue)
            {
                var start = command.StartDate.Value.Date;
                var end = command.EndDate.Value.Date;

                if (start >= end)
                {
                    errors.Add("Start date must be earlier than end date.");
                }
                else
                {
                    // Check for overlapping financial years (exclude current record by UUID)
                    var overlaps = await _repository.ExistsAsync(x =>
                        x.UUID != command.UUID &&
                        x.StartDate != null &&
                        x.EndDate != null &&
                        x.StartDate <= end &&
                        x.EndDate >= start);

                    if (overlaps)
                        errors.Add("The provided date range overlaps with an existing financial year.");
                }
            }

            if (errors.Any())
            {
                throw new ValidationException(string.Join(" ", errors));
            }

            await Task.CompletedTask;
        }
    }
}
   

  