using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.Commands.WL;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.DTO.WL;
using Upgrow.Application.IServices.Master;
using Upgrow.Application.IServices.WL;
using Upgrow.Application.Services.Master;
using Upgrow.Domain.Entities;
using Upgrow.Domain.Entities.WL;
using Upgrow.Domain.IRepositories;
using Upgrow.Domain.IRepositories.Master;
using Upgrow.Domain.IRepositories.WL;

namespace Upgrow.Application.Services.WL
{
    public class WLPasswordPolicyService : MasterServiceBase<WL_PasswordPolicy, WLPasswordPolicyDto, WLPasswordPolicyCommand>, IWLPasswordPolicyService
    {
        private readonly IWLPasswordPolicyRepository _passwordPolicyRepository;
        public WLPasswordPolicyService(IMasterRepository<WL_PasswordPolicy> repository, IMapper mapper, IWLPasswordPolicyRepository passwordPolicyRepository)
          : base(repository, mapper)
        {
            _passwordPolicyRepository = passwordPolicyRepository;
        }

        public async Task<WLPasswordPolicyDto?> GetFirstAsync()
        {
            var entity = await _passwordPolicyRepository.GetFirstOrDefaultAsync();
            return entity == null ? null : _mapper.Map<WLPasswordPolicyDto>(entity);
        }

        protected override Expression<Func<WL_PasswordPolicy, bool>>? BuildSearchFilter(string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override async Task<bool> IsDuplicateAsync(WLPasswordPolicyCommand command)
        {
            return await _repository.ExistsAsync(x => x.UUID != command.UUID);
        }
    }
}
