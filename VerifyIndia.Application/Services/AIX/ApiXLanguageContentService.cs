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
    public class ApiXLanguageContentService : MasterServiceBase<ApiXLanguageContent, ApiXLanguageContentDto, ApiXLanguageContentCommand>, IApiXLanguageContentService
    {
        public ApiXLanguageContentService(IMasterRepository<ApiXLanguageContent> repository, IMapper mapper)
            : base(repository, mapper) { }

        public async Task<List<ApiXLanguageContentDto>> GetAllAsync()
        {
            var entities = await _repository.FindAllAsync(x => true);
            return _mapper.Map<List<ApiXLanguageContentDto>>(entities);
        }
        public async Task<List<ApiXLanguageContentDto>> GetAllActiveAsync()
        {
            var entities = await _repository.GetAllActiveAsync();
            return _mapper.Map<List<ApiXLanguageContentDto>>(entities);
        }

        public async Task<ApiXLanguageContentDto> SaveAndReturnAsync(ApiXLanguageContentCommand command, string userUuid, string ip)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            await ValidateAsync(command);
            if (await IsDuplicateAsync(command))
            {
                // Surface a clear message; controller will return this to client
                throw new InvalidOperationException("A snippet for this language already exists for this API version.");
            }
            if (string.IsNullOrWhiteSpace(command.UUID))
            {
                var entity = _mapper.Map<ApiXLanguageContent>(command);
                entity.UUID = Utils.GetUUID();
                entity.IsActive = true;
                command.IsActive = true;
                OnBeforeCreate(entity, userUuid, ip);

                // keep language title if provided
                await _repository.AddAsync(entity);

                return _mapper.Map<ApiXLanguageContentDto>(entity);
            }
            else
            {
                var entity = await _repository.GetByUuidAsync(command.UUID) ?? throw new Exception("Record not found");
                _mapper.Map(command, entity);

                OnBeforeUpdate(entity, userUuid, ip);

                await _repository.UpdateAsync(entity);

                return _mapper.Map<ApiXLanguageContentDto>(entity);
            }
        }

        protected override async Task<bool> IsDuplicateAsync(ApiXLanguageContentCommand command)
        {
            return await _repository.ExistsAsync(x =>
                (x.ApiXVersionUUID ?? "").Trim().ToLower() ==
                (command.ApiXVersionUUID ?? "").Trim().ToLower() &&
                (x.LanguageContent ?? "").Trim().ToLower() ==
                (command.LanguageContent ?? "").Trim().ToLower() &&
                x.UUID != command.UUID
            );
        }

        public Task<bool> CheckDuplicateAsync(ApiXLanguageContentCommand command)
        {
            return IsDuplicateAsync(command);
        }

        protected override Expression<Func<ApiXLanguageContent, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            return x =>
                (x.LanguageContent != null && x.LanguageContent.ToLower().Contains(searchTerm)) ||
                (x.LanguageUUID != null && x.LanguageContent.ToLower().Contains(searchTerm));
        }

        protected override Func<IQueryable<ApiXLanguageContent>, IOrderedQueryable<ApiXLanguageContent>>? BuildSortExpression(string? sortColumn, string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";
            return sortColumn?.ToLower() switch
            {
                "language" => q => isAsc ? q.OrderBy(x => x.LanguageContent) : q.OrderByDescending(x => x.LanguageContent),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }

        public async Task<ApiXLanguageContentDto?> GetByVersionAndLanguageAsync(string versionUUID, string languageUUID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(versionUUID) || string.IsNullOrWhiteSpace(languageUUID))
                    return null;

                var contents = await _repository.FindAllAsync(
                    x => x.ApiXVersionUUID == versionUUID &&
                         x.LanguageUUID == languageUUID &&
                         x.IsActive);

                var content = contents.FirstOrDefault();
                return _mapper.Map<ApiXLanguageContentDto>(content);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error fetching language content: {ex.Message}", ex);
            }
        }

        public async Task<List<ApiXLanguageContentDto>> GetByVersionAsync(string versionUUID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(versionUUID))
                    return new List<ApiXLanguageContentDto>();

                var contents = await _repository.FindAllAsync(
                    x => x.ApiXVersionUUID == versionUUID && x.IsActive);

                return _mapper.Map<List<ApiXLanguageContentDto>>(contents);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error fetching language contents: {ex.Message}", ex);
            }
        }
    }
}
