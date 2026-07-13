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
    public class CredentialsWhatsappServices : MasterServiceBase<Credential_Whatsapp, CredentialsWhatsappDto, CredentialsWhatsappCommand>, ICredentialsWhatsappServices
    {
        public CredentialsWhatsappServices(IMasterRepository<Credential_Whatsapp> repository, IMapper mapper)
            : base(repository, mapper) { }

        protected override async Task<bool> IsDuplicateAsync(CredentialsWhatsappCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.MobileNo.ToLower().Trim() == command.MobileNo.ToLower().Trim() &&
                x.APIKey.ToLower().Trim() == command.APIKey.ToLower().Trim() &&
                x.SenderName.ToLower().Trim() == command.SenderName.ToLower().Trim() &&
                x.UUID != command.UUID);
        }
        protected override Expression<Func<Credential_Whatsapp, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            return x =>
                (x.MobileNo != null && x.MobileNo.ToLower().Contains(searchTerm)) ||
                (x.APIKey != null && x.APIKey.ToLower().Contains(searchTerm)) ||
                (x.AccessToken != null && x.AccessToken.ToLower().Contains(searchTerm)) ||
                (x.SenderName != null && x.SenderName.ToLower().Contains(searchTerm));
        }
        protected override Func<IQueryable<Credential_Whatsapp>, IOrderedQueryable<Credential_Whatsapp>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";
            return sortColumn?.ToLower() switch
            {
                "mobileno" => q => isAsc ? q.OrderBy(x => x.MobileNo) : q.OrderByDescending(x => x.MobileNo),
                "apikey" => q => isAsc ? q.OrderBy(x => x.APIKey) : q.OrderByDescending(x => x.APIKey),
                "secret" => q => isAsc ? q.OrderBy(x => x.AccessToken) : q.OrderByDescending(x => x.AccessToken),
                "sendername" => q => isAsc ? q.OrderBy(x => x.SenderName) : q.OrderByDescending(x => x.SenderName),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }

        protected override async Task UpdateAsync(
   CredentialsWhatsappCommand command,
   string userUuid,
   string ip,
   bool saveChanges = true)
        {
            var entity = await _repository.GetByUuidAsync(command.UUID!)
                ?? throw new Exception("Record not found");

            if (string.IsNullOrWhiteSpace(command.AccessToken) || command.AccessToken == Constants.SecretMaskConstants.MaskedSecretValue)
            {
                command.AccessToken = entity.AccessToken;
            }
            _mapper.Map(command, entity);

            await _repository.UpdateAsync(entity, saveChanges);
        }
    }
}
