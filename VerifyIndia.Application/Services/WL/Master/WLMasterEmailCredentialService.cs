using AutoMapper;
using System;
using System.Collections.Generic;
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
    public class WLMasterEmailCredentialService : MasterServiceBase<WL_MasterEmailCredential, WLMasterEmailCredentialDto, WLMasterEmailCredentialCommand>, IWLMasterEmailCredentialService
    {
        public WLMasterEmailCredentialService(IMasterRepository<WL_MasterEmailCredential> repository, IMapper mapper)
          : base(repository, mapper) { }

        // Define which field to check for duplicates
        protected override async Task<bool> IsDuplicateAsync(WLMasterEmailCredentialCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.EmailAddress!.ToLower().Trim() == command.EmailAddress.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        // Define which fields to search
        protected override Expression<Func<WL_MasterEmailCredential, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.EmailAddress != null && x.EmailAddress.ToLower().Contains(searchTerm)) ||
                 (x.HostServiceProvider != null && x.HostServiceProvider.ToLower().Contains(searchTerm)) ||
                  (x.Port != null && x.Port.ToLower().Contains(searchTerm)) ||
                (x.SMTP != null && x.SMTP.ToLower().Contains(searchTerm));
        }
        // Define sorting (optional - remove if default Id sorting is fine)
        protected override Func<IQueryable<WL_MasterEmailCredential>, IOrderedQueryable<WL_MasterEmailCredential>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "name" => q => isAsc ? q.OrderBy(x => x.EmailAddress) : q.OrderByDescending(x => x.EmailAddress),
                "smtp" => q => isAsc ? q.OrderBy(x => x.SMTP) : q.OrderByDescending(x => x.SMTP),
                "port" => q => isAsc ? q.OrderBy(x => x.Port) : q.OrderByDescending(x => x.Port),
                "hostserviceprovider" => q => isAsc ? q.OrderBy(x => x.HostServiceProvider) : q.OrderByDescending(x => x.HostServiceProvider),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };


        }
    }
}
    
