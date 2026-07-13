using AutoMapper;
using Upgrow.Application.DTO.AIX;
using Upgrow.Application.DTO.Api;
using Upgrow.Application.IServices.Api;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.Services.Api
{
    public class ReqResSchemaFieldService : IReqResSchemaFieldService
    {
        private readonly IMasterRepository<ReqResSchemaFields> _schemaFieldRepository;
        private readonly IMapper _mapper;

        public ReqResSchemaFieldService(
            IMasterRepository<ReqResSchemaFields> schemaFieldRepository,
            IMapper mapper)
        {
            _schemaFieldRepository = schemaFieldRepository;
            _mapper = mapper;
        }

        public async Task<List<ReqResSchemaFieldDto>> GetBySchemaUuidAsync(string schemaUuid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(schemaUuid))
                    return new List<ReqResSchemaFieldDto>();

                var fields = await _schemaFieldRepository.FindAllAsync(
                    x => x.SchemaUUID == schemaUuid && x.IsActive);

                return _mapper.Map<List<ReqResSchemaFieldDto>>(
                    fields.OrderBy(f => f.DisplayOrder).ToList());
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error fetching schema fields: {ex.Message}", ex);
            }
        }
    }
}