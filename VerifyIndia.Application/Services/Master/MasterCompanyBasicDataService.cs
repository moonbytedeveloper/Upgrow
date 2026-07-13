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
using VerifyIndia.Domain.IRepositories.Master;

namespace VerifyIndia.Application.Services.Master
{
    public class MasterCompanyBasicDataService : MasterServiceBase<Master_CompanyBasicData, MasterCompanyBasicDataDto, MasterCompanyBasicDataCommand>, IMasterCompanyBasicDataService
    {
        private readonly IMasterCompanyBasicDataRepository _companyRepo;
        public MasterCompanyBasicDataService(IMasterCompanyBasicDataRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _companyRepo = repository;
        }

        public async Task<MasterCompanyBasicDataDto?> GetFirstAsync()
        {
            var entity = await _companyRepo.GetFirstOrDefaultAsync();
            return entity == null ? null : _mapper.Map<MasterCompanyBasicDataDto>(entity);
        }

        protected override async Task<bool> IsDuplicateAsync(MasterCompanyBasicDataCommand command)
        {
            return await _repository.ExistsAsync(x => x.CompName!.ToLower().Trim() == command.CompName.ToLower().Trim() &&
           x.UUID != command.UUID);
        }
        protected override Expression<Func<Master_CompanyBasicData, bool>>? BuildSearchFilter(string searchTerm)
        {
            throw new NotImplementedException();
        }
    }
}
