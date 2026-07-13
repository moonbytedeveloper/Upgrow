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
    public class CredentialsSMSGatewayServices : MasterServiceBase<Credential_SMS_Gateway, CredentialsSMSGatewayDto, CredentialsSMSGatewayCommand>, ICredentialsSMSGatewayServices
    {
        public CredentialsSMSGatewayServices(IMasterRepository<Credential_SMS_Gateway> repository, IMapper mapper)
            : base(repository, mapper) { }

        protected override async Task<bool> IsDuplicateAsync(CredentialsSMSGatewayCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.SenderId.ToLower().Trim() == command.SenderId.ToLower().Trim() &&
                x.APIKey.ToLower().Trim() == command.APIKey.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        protected override Expression<Func<Credential_SMS_Gateway, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            return x =>
                (x.SenderId != null && x.SenderId.ToLower().Contains(searchTerm)) ||
                (x.APIKey != null && x.APIKey.ToLower().Contains(searchTerm));
        }

        protected override Func<IQueryable<Credential_SMS_Gateway>, IOrderedQueryable<Credential_SMS_Gateway>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";
            return sortColumn?.ToLower() switch
            {
                "senderid" => q => isAsc ? q.OrderBy(x => x.SenderId) : q.OrderByDescending(x => x.SenderId),
                "apikey" => q => isAsc ? q.OrderBy(x => x.APIKey) : q.OrderByDescending(x => x.APIKey),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }
    }
}