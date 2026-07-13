using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Credential;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.DTO.Credential;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.IServices.Credential;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Application.Services.Master;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.Credential
{
    public class PaymentGatewayCredentialService : MasterServiceBase<PaymentGatewayCredential, PaymentGatewayCredentialDto, PaymentGatewayCredentialCommand>, IPaymentGatewayCredentialService
    {
        public PaymentGatewayCredentialService(IMasterRepository<PaymentGatewayCredential> repository, IMapper mapper) : base(repository, mapper)
        {
        }

        protected override async Task<bool> IsDuplicateAsync(PaymentGatewayCredentialCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.Name.ToLower().Trim() == command.Name.ToLower().Trim() &&
                x.ApiKey.ToLower().Trim() == command.ApiKey.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        protected override Expression<Func<PaymentGatewayCredential, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            return x =>
                (x.Name != null && x.Name.ToLower().Contains(searchTerm)) ||
                (x.ApiKey != null && x.ApiKey.ToLower().Contains(searchTerm)) ||
                (x.MerchantId != null && x.MerchantId.ToLower().Contains(searchTerm)) ||
                (x.Code != null && x.Code.ToLower().Contains(searchTerm));
        }

        protected override Func<IQueryable<PaymentGatewayCredential>, IOrderedQueryable<PaymentGatewayCredential>>? BuildSortExpression(
           string? sortColumn,
           string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";
            return sortColumn?.ToLower() switch
            {
                "name" => q => isAsc ? q.OrderBy(x => x.Name) : q.OrderByDescending(x => x.Name),
                "code" => q => isAsc ? q.OrderBy(x => x.Code) : q.OrderByDescending(x => x.Code),
                "apikey" => q => isAsc ? q.OrderBy(x => x.ApiKey) : q.OrderByDescending(x => x.ApiKey),
                "merchantId" => q => isAsc ? q.OrderBy(x => x.MerchantId) : q.OrderByDescending(x => x.MerchantId),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }
        protected override void OnBeforeCreate(
       PaymentGatewayCredential entity,
       string userUuid,
       string ip)
        {
            entity.Code = GenerateCode(entity.Name);
        }
        private static string GenerateCode(string value)
        {
            return Regex.Replace(
                    value.Trim().ToUpperInvariant(),
                    @"[^A-Z0-9]+",
                    "_")
                .Trim('_');
        }
        protected override async Task UpdateAsync(
 PaymentGatewayCredentialCommand command,
 string userUuid,
 string ip,
 bool saveChanges = true)
        {
            var entity = await _repository.GetByUuidAsync(command.UUID!)
                ?? throw new Exception("Record not found");

            if (string.IsNullOrWhiteSpace(command.EncryptedKeySecret) || command.EncryptedKeySecret == Constants.SecretMaskConstants.MaskedSecretValue)
            {
                command.EncryptedKeySecret = entity.EncryptedKeySecret;
            }
            _mapper.Map(command, entity);

            await _repository.UpdateAsync(entity, saveChanges);
        }
    }
}
