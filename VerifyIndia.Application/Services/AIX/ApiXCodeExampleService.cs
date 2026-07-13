using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.AIX;
using VerifyIndia.Application.DTO.AIX;
using VerifyIndia.Application.IServices.AIX;
using VerifyIndia.Application.Services.Master;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.AIX
{
    public class ApiXCodeExampleService : MasterServiceBase<ApiXCodeExample, ApiXCodeExampleDto, ApiXCodeExampleCommand>, IApiXCodeExampleService
    {
        public ApiXCodeExampleService(IMasterRepository<ApiXCodeExample> repository, IMapper mapper)
            : base(repository, mapper)
        {
        }
        public async Task<List<ApiXCodeExampleDto>> GetAllActiveAsync()
        {
            var entities = await _repository.GetAllActiveAsync();
            return _mapper.Map<List<ApiXCodeExampleDto>>(entities);
        }

        // Duplicate when same FieldName exists for same SchemaUUID (exclude self when updating)
        protected override async Task<bool> IsDuplicateAsync(ApiXCodeExampleCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.ErrorTitle == command.ErrorTitle &&
                x.UUID != command.UUID);
        }

        

        // Search by FieldName, DataType or Constraints
        protected override Expression<Func<ApiXCodeExample, bool>>? BuildSearchFilter(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return null;

            // request.Search is already lowered in caller; compare using ToLower for provider compatibility
            return e =>
                (e.ErrorTitle != null && e.ErrorTitle.ToLower().Contains(searchTerm)) ;
        }

        public async Task<ResponseExampleDto?> GetByUuidAsync(string uuid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(uuid))
                    return null;

                var example = await _repository.GetByUuidAsync(uuid);
                return example == null ? null : _mapper.Map<ResponseExampleDto>(example);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error fetching code example by UUID: {ex.Message}", ex);
            }
        }


        public async Task<List<ResponseExampleDto>> GetByStatusCodeUuidAsync(string statusCodeUuid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(statusCodeUuid))
                    return new List<ResponseExampleDto>();

                var examples = await _repository.FindAllAsync(
                    x => x.StatusCodeUUID == statusCodeUuid && x.IsActive);

                return _mapper.Map<List<ResponseExampleDto>>(examples);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error fetching code examples by status code UUID: {ex.Message}", ex);
            }
        }
    }
}

    
