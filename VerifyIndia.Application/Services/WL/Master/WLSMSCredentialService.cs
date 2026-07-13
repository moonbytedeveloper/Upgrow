using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.WL.Master;
using VerifyIndia.Application.DTO.WL.Master;
using VerifyIndia.Application.IServices.WL;
using VerifyIndia.Application.IServices.WL.Master;
using VerifyIndia.Application.Services.Master;
using VerifyIndia.Domain.Entities.WL.Master;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.WL.Master
{
    public class WLSMSCredentialService : MasterServiceBase<WL_SMSCredential, WLSMSCredentialDto, WLSMSCredentialCommand>, IWLSMSCredentialService
    {
        public WLSMSCredentialService(IMasterRepository<WL_SMSCredential> repository, IMapper mapper)
            : base(repository, mapper) { }

        // Define which field to check for duplicates
        protected override string EntityDisplayName => "SMS Credential";
        protected override async Task<bool> IsDuplicateAsync(WLSMSCredentialCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.ApiKey!.ToLower().Trim() == command.ApiKey.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        // Define which fields to search
        protected override Expression<Func<WL_SMSCredential, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.ApiKey != null && x.ApiKey.ToLower().Contains(searchTerm)) ||
                (x.SenderId != null && x.SenderId.ToLower().Contains(searchTerm));
        }

        // Define sorting (optional - remove if default Id sorting is fine)
        protected override Func<IQueryable<WL_SMSCredential>, IOrderedQueryable<WL_SMSCredential>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "apikey" => q => isAsc ? q.OrderBy(x => x.ApiKey) : q.OrderByDescending(x => x.ApiKey),
                "senderid" => q => isAsc ? q.OrderBy(x => x.SenderId) : q.OrderByDescending(x => x.SenderId),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }


    }
}
   

