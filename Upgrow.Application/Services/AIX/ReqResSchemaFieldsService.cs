
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.AIX;
using Upgrow.Application.DTO.AIX;
using Upgrow.Application.IServices.AIX;
using Upgrow.Application.Services.Master;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.Services.AIX
{
    public class ReqResSchemaFieldsService : MasterServiceBase<ReqResSchemaFields, ReqResSchemaFieldDto, ReqResSchemaFieldCommand>, IReqResSchemaFieldsService
    {
        public ReqResSchemaFieldsService(IMasterRepository<ReqResSchemaFields> repository, IMapper mapper)
            : base(repository, mapper)
        {
        }

        public async Task<List<ReqResSchemaFieldDto>> GetAllAsync()
        {
            var entities = await _repository.FindAllAsync(x => true);
            return _mapper.Map<List<ReqResSchemaFieldDto>>(entities);
        }
        public async Task<ReqResSchemaFieldDto> SaveAndReturnAsync(ReqResSchemaFieldCommand command, string userUuid, string ip)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            await SaveAsync(command, userUuid, ip);

            if (string.IsNullOrWhiteSpace(command.UUID))
                throw new InvalidOperationException("Save failed to produce UUID.");

            var dto = await GetByUuidAsync(command.UUID);
            if (dto == null)
                throw new InvalidOperationException("Failed to retrieve saved field.");

            return dto;
        }

        public async Task<List<ReqResSchemaFieldDto>> GetAllActiveAsync()
        {
            var entities = await _repository.GetAllActiveAsync();
            return _mapper.Map<List<ReqResSchemaFieldDto>>(entities);
        }
        // Duplicate when same FieldName exists for same SchemaUUID (exclude self when updating)
        protected override async Task<bool> IsDuplicateAsync(ReqResSchemaFieldCommand command)
        {
            return await _repository.ExistsAsync(x =>
                 x.SchemaUUID == command.SchemaUUID &&
                  x.FieldName == command.FieldName &&
                x.ParentUUID == command.ParentUUID &&
                x.UUID != command.UUID);
        }

        public Task<bool> CheckDuplicateAsync(ReqResSchemaFieldCommand command)
        {
            return IsDuplicateAsync(command);
        }

        // Search by FieldName, DataType or Constraints
        protected override Expression<Func<ReqResSchemaFields, bool>>? BuildSearchFilter(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return null;

            // request.Search is already lowered in caller; compare using ToLower for provider compatibility
            return e =>
                (e.FieldName != null && e.FieldName.ToLower().Contains(searchTerm)) ||
                (e.DataType != null && e.DataType.ToLower().Contains(searchTerm)) ||
                (e.Constraints != null && e.Constraints.ToLower().Contains(searchTerm));
        }
    }
}