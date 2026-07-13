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
using VerifyIndia.Domain.IRepositories.Master;

namespace VerifyIndia.Application.Services.Master
{
    public class PasswordPolicyService : MasterServiceBase<Password_Policy, PasswordPolicyDto, PasswordPolicyCommand>, IPasswordPolicyService
    {
        private readonly IPasswordPolicyRepository _passwordPolicyRepository;
        public PasswordPolicyService(IMasterRepository<Password_Policy> repository, IMapper mapper,IPasswordPolicyRepository passwordPolicyRepository)
          : base(repository, mapper) 
        {
            _passwordPolicyRepository = passwordPolicyRepository;
        }

        public async Task<PasswordPolicyDto?> GetFirstAsync()
        {
            var entity = await _passwordPolicyRepository.GetFirstOrDefaultAsync();
            return entity == null ? null : _mapper.Map<PasswordPolicyDto>(entity);
        }

        protected override Expression<Func<Password_Policy, bool>>? BuildSearchFilter(string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override async Task<bool> IsDuplicateAsync(PasswordPolicyCommand command)
        {
            return await _repository.ExistsAsync(x => x.UUID != command.UUID);
        }
    }
}
