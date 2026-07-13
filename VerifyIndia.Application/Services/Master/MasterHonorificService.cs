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
    public class MasterHonorificService : MasterServiceBase<Master_Honorific, MasterHonorificDto, MasterHonorificCommand>, IMasterHonorificService
    {
        public MasterHonorificService(IMasterRepository<Master_Honorific> repository, IMapper mapper)
            : base(repository, mapper) { }

        // Define which field to check for duplicates
        protected override async Task<bool> IsDuplicateAsync(MasterHonorificCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.Title!.ToLower().Trim() == command.Title.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        // Define which fields to search
        protected override Expression<Func<Master_Honorific, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.Title != null && x.Title.ToLower().Contains(searchTerm));
        }

        // Define sorting (optional - remove if default Id sorting is fine)
        protected override Func<IQueryable<Master_Honorific>, IOrderedQueryable<Master_Honorific>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "name" => q => isAsc ? q.OrderBy(x => x.Title) : q.OrderByDescending(x => x.Title),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }

        /*public async Task<Dictionary<string, string>> GetNameAsync(IEnumerable<string?> userUuids)
        {
            var uuids = userUuids
                .Where(u => !string.IsNullOrWhiteSpace(u))
                .Distinct()
                .ToList();

            if (!uuids.Any())
                return new Dictionary<string, string>();

            var result = new Dictionary<string, string>();

            foreach (var uuid in uuids)
            {
                var entity = await _repository.GetByUuidAsync(uuid!);

                if (entity != null && entity.IsActive)
                {
                    result[entity.UUID!] = entity.Title ?? string.Empty;
                }
            }

            return result;
        }*/
    }
}
