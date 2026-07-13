using AutoMapper;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using VerifyIndia.Application.Commands;
using VerifyIndia.Application.DTO.CustomerPanel;
using VerifyIndia.Application.IServices;
using VerifyIndia.Application.Services.Master;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;
using VerifyIndia.Application.DTO;

namespace VerifyIndia.Application.Services
{
    // Follow project pattern: inherit MasterServiceBase<TEntity, TDto, TCommand>
    public class PinnedService : MasterServiceBase<Pinned_Services, PinnedServiceDto, PinnedServiceCommand>, IPinnedService
    {
        public PinnedService(IMasterRepository<Pinned_Services> repository, IMapper mapper)
            : base(repository, mapper)
        {
        }

        // Expose a simple DTO-based helper used by DashboardService / controllers
        public async Task<List<PinnedServiceDto>> GetAllActiveAsync()
        {
            var entities = await _repository.GetAllActiveAsync();
            return _mapper.Map<List<PinnedServiceDto>>(entities);
        }

        
        public Task<bool> IsPinnedAsync(string apiUuid, string? customerUuid = null)
        {
            if (string.IsNullOrWhiteSpace(apiUuid))
                return Task.FromResult(false);

            if (string.IsNullOrWhiteSpace(customerUuid))
                return _repository.ExistsAsync(x => x.ApiUUID == apiUuid && x.IsActive);

            return _repository.ExistsAsync(x => x.ApiUUID == apiUuid && x.CustomerUUID == customerUuid && x.IsActive);
        }

        // Required by MasterServiceBase: duplicate check
        protected override async Task<bool> IsDuplicateAsync(PinnedServiceCommand command)
        {
            if (command == null) return false;

            return await _repository.ExistsAsync(x =>
                x.ApiUUID == command.ApiUUID &&
                (command.CustomerUUID == null || x.CustomerUUID == command.CustomerUUID) &&
                x.UUID != command.UUID);
        }

        // Optional: search filter for admin/listing pages
        protected override Expression<Func<Pinned_Services, bool>>? BuildSearchFilter(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm)) return null;
            searchTerm = searchTerm.Trim().ToLower();
            return x =>
                (x.ApiUUID != null && x.ApiUUID.ToLower().Contains(searchTerm)) ||
                (x.CustomerUUID != null && x.CustomerUUID.ToLower().Contains(searchTerm));
        }
    }
}