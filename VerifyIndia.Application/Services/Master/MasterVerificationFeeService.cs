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
using VerifyIndia.Domain.Entities.Registration;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.Master
{
    public class MasterVerificationFeeService : MasterServiceBase<Master_VerificationFee, MasterVerificationFeeDto, MasterVerificationFeeCommand>, IMasterVerificationFeeService
    {
        public MasterVerificationFeeService(IMasterRepository<Master_VerificationFee> repository, IMapper mapper) : base(repository, mapper)
        {
        }
        protected override async Task<bool> IsDuplicateAsync(MasterVerificationFeeCommand command)
        {
            return await _repository.ExistsAsync(x =>
               x.VerificationType!.ToLower().Trim() == command.VerificationType!.ToLower().Trim() &&
               x.Amount == command.Amount &&
               x.UUID != command.UUID);
        }
        protected override Expression<Func<Master_VerificationFee, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.VerificationType != null && x.VerificationType.ToLower().Contains(searchTerm)) ||
                (x.Amount.ToString().Contains(searchTerm));
        }

        // Define sorting (optional - remove if default Id sorting is fine)
        protected override Func<IQueryable<Master_VerificationFee>, IOrderedQueryable<Master_VerificationFee>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "verificationtype" => q => isAsc ? q.OrderBy(x => x.VerificationType) : q.OrderByDescending(x => x.VerificationType),
                "amount" => q => isAsc ? q.OrderBy(x => x.Amount) : q.OrderByDescending(x => x.Amount),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }

        protected override void OnBeforeCreate(
       Master_VerificationFee entity,
       string userUuid,
       string ip)
        {
            entity.CreatedAt = DateTimeOffset.UtcNow;
        }

        protected override void OnBeforeUpdate(
            Master_VerificationFee entity,
            string userUuid,
            string ip)
        {
            entity.UpdatedAt = DateTimeOffset.UtcNow;
        }
    }
}
