using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.WL.Master;
using VerifyIndia.Application.DTO.WL.Master;
using VerifyIndia.Application.IServices.WL.Master;
using VerifyIndia.Application.Services.Master;
using VerifyIndia.Domain.Entities.WL.Master;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.WL.Master
{
    public class WLWhatsappCredentialService : MasterServiceBase<WL_WhatsappCredential, WLWhatsappCredentialDto, WLWhatsappCredentialCommand>, IWLWhatsappCredentialService
    {
        public WLWhatsappCredentialService(IMasterRepository<WL_WhatsappCredential> repository, IMapper mapper)
            : base(repository, mapper) { }

        // Define which field to check for duplicates
        protected override string EntityDisplayName => "Whatsapp Credential";
        protected override async Task<bool> IsDuplicateAsync(WLWhatsappCredentialCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.SenderName!.ToLower().Trim() == command.SenderName.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        // Define which fields to search
        protected override Expression<Func<WL_WhatsappCredential, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.SenderName != null && x.SenderName.ToLower().Contains(searchTerm)) ||
                (x.MobileNumber != null && x.MobileNumber.ToLower().Contains(searchTerm));
        }

        // Define sorting (optional - remove if default Id sorting is fine)
        protected override Func<IQueryable<WL_WhatsappCredential>, IOrderedQueryable<WL_WhatsappCredential>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "sendername" => q => isAsc ? q.OrderBy(x => x.SenderName) : q.OrderByDescending(x => x.SenderName),
                "mobilenumber" => q => isAsc ? q.OrderBy(x => x.MobileNumber) : q.OrderByDescending(x => x.MobileNumber),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }


    }
}
   

