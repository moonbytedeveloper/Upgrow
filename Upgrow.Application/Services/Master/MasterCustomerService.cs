using AuthenticateIndia.Shared.Constants.Registration;
using AutoMapper;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.DTO.Customer;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.DTO.WL;
using Upgrow.Application.IServices.Master;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;
using Upgrow.Domain.IRepositories.Master;

namespace Upgrow.Application.Services.Master
{
    public class MasterCustomerService : MasterServiceBase<Master_Customer, MasterCustomerDto, MasterCustomerCommand>, IMasterCustomerService
    {
        private readonly IMasterCustomerRepository _masterCustomerRepository;
        private readonly ITenantRepository _tenantRepository;
        public MasterCustomerService(IMasterRepository<Master_Customer> repository, IMapper mapper, IMasterCustomerRepository masterCustomerRepository, ITenantRepository tenantRepository) : base(repository, mapper)
        {

            _masterCustomerRepository = masterCustomerRepository;
            _tenantRepository = tenantRepository;

        }

        public async Task<Master_Customer?> GetByAadhaarHashAsync(
            string aadhaarHash)
        {
            return await _masterCustomerRepository
                .GetByAadhaarHashAsync(
                    aadhaarHash);
        }

        public async Task<Master_Customer> CreateCustomerAsync(
    string mobile)
        {
            var customer = new Master_Customer
            {
                UUID = Utils.GetUUID(),
                Mobile = mobile,
                CurrentStep = RegistrationSteps.SELECT_ACCOUNT_TYPE,
                IsActive = true
            };

            await _masterCustomerRepository.AddAsync(
                customer);

            return customer;
        }
        public async Task<Master_Customer?> GetReferralAgentAsync(
    string mobile)
        {
            return await _masterCustomerRepository
                .GetReferralAgentAsync(
                    mobile);
        }
        public async Task<Master_Customer?> GetByMobileAndClientIdentifierAsync(string mobile, string TenantIdentifier)
        {
            var tenant = await _tenantRepository.GetTenantDetailsByIdentifierAsync(TenantIdentifier);

            var customerEntity = await _masterCustomerRepository.GetByMobileAndClientIdAsync(mobile, tenant.Id);
            if (customerEntity == null) return null;
            return customerEntity;            
        }
        public async Task<Master_Customer?> GetByMobileAsync(string mobile)
        {
            var customerEntity = await _masterCustomerRepository.GetByMobileAsync(mobile);
            if (customerEntity == null) return null;
            return customerEntity;          
        }
        public async Task<Master_Customer?> GetCustomerByUUID(string UUID)
        {
            var customerEntity = await _masterCustomerRepository.GetByUuidAsync(UUID);
            if (customerEntity == null) return null;
            return customerEntity;
        }

        public async Task<Master_Customer> UpdateCustomerAsync(Master_Customer customer, bool saveChanges = true)
        {
            await _masterCustomerRepository.UpdateAsync(customer, saveChanges);
            return customer;
        }
        public async Task<List<MasterCustomerDto>> GetAllAsync(Expression<Func<Master_Customer, bool>> predicate)
        {
            var entities = await _masterCustomerRepository.GetAllActiveAsync();

            var filtered = entities.AsQueryable().Where(predicate); // ✅ APPLY FILTER

            return filtered.Select(t => new MasterCustomerDto
            {
                UUID = t.UUID,
                FName = t.FName,
                LName = t.LName,
            }).ToList();
        }

        protected override async Task<bool> IsDuplicateAsync(MasterCustomerCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.FName!.ToLower().Trim() == command.FName.ToLower().Trim() &&
                x.MName!.ToLower().Trim() == command.MName.ToLower().Trim() &&
                x.LName!.ToLower().Trim() == command.LName.ToLower().Trim() &&
                x.Mobile!.ToLower().Trim() == command.Mobile.ToLower().Trim() &&
                x.Email!.ToLower().Trim() == command.Email.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        protected override Expression<Func<Master_Customer, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            return x =>
                (x.FName != null && x.FName.ToLower().Contains(searchTerm))||
                (x.MName != null && x.MName.ToLower().Contains(searchTerm)) ||
                (x.LName != null && x.LName.ToLower().Contains(searchTerm)) ||
                (x.Email != null && x.Email.ToLower().Contains(searchTerm)) ||
                (x.IndustryUUID != null && x.IndustryUUID.ToLower().Contains(searchTerm)) ||
                (x.CityUUID != null && x.CityUUID.ToLower().Contains(searchTerm)) ||
                (x.Mobile != null && x.Mobile.ToLower().Contains(searchTerm));
        }
        protected override Func<IQueryable<Master_Customer>, IOrderedQueryable<Master_Customer>>? BuildSortExpression(
       string? sortColumn,
       string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "fullname" => q => isAsc ? q.OrderBy(x => x.FName) : q.OrderByDescending(x => x.FName),
                "email" => q => isAsc ? q.OrderBy(x => x.Email) : q.OrderByDescending(x => x.Email),
                "city" => q => isAsc ? q.OrderBy(x => x.CityUUID) : q.OrderByDescending(x => x.CityUUID),
                "industry" => q => isAsc ? q.OrderBy(x => x.IndustryUUID) : q.OrderByDescending(x => x.IndustryUUID),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }
        public async Task ConvertToAgentAsync(string uuid, string userId)
        {
            var data = await _repository.GetByUuidAsync(uuid);

            if (data == null)
                throw new Exception("Customer not found.");

            data.IsAgent = true;

            await _repository.UpdateAsync(data);
        }

        public async Task ConvertToAgentHeadAsync(string uuid, string userId)
        {
            var data = await _repository.GetByUuidAsync(uuid);

            if (data == null)
                throw new Exception("Customer not found.");

            data.IsAgent = true;
            data.IsAgentHead = true;

            await _repository.UpdateAsync(data);
        }
    }
}
