using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.Commands.WL;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.DTO.WL;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Application.IServices.WL;
using VerifyIndia.Application.Services.Master;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.Entities.WL;
using VerifyIndia.Domain.IRepositories;
using VerifyIndia.Domain.IRepositories.Master;
using VerifyIndia.Domain.IRepositories.WL;

namespace VerifyIndia.Application.Services.WL
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
