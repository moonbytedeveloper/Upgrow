
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.AIX;
using VerifyIndia.Application.Commands.Api;
using VerifyIndia.Application.DTO.AIX;
using VerifyIndia.Application.DTO.Api;
using VerifyIndia.Application.IServices.AIX;
using VerifyIndia.Application.Services.Master;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.AIX
{
    // Service implemented on top of MasterServiceBase to keep consistent patterns with other master services.
    public class ApiXVersionService : MasterServiceBase<ApiXVersion, ApixVersionDto, ApixVersionCommand>, IApiXVersionService
    {
        private readonly IMasterRepository<ApiXHeaders> _headersRepository;
        private readonly IMasterRepository<RequestSchema> _requestSchemaRepository;
        private readonly IMasterRepository<ResponseSchema> _responseSchemaRepository;
        private readonly IMasterRepository<ReqResSchemaFields> _reqResSchemaFieldsRepository;
        private readonly IMasterRepository<ApiXCodeMapper> _codeMapperRepository;
        private readonly IMasterRepository<ApiXLanguageContent> _languageSnippetRepository;
        public ApiXVersionService(
    IMasterRepository<ApiXVersion> repository,
    IMasterRepository<ApiXHeaders> headersRepository,
    IMasterRepository<RequestSchema> requestSchemaRepository,
    IMasterRepository<ResponseSchema> responseSchemaRepository,
    IMasterRepository<ReqResSchemaFields> reqResSchemaFieldsRepository,
    IMasterRepository<ApiXCodeMapper> codeMapperRepository,
    IMasterRepository<ApiXLanguageContent> languageSnippetRepository,
    IMapper mapper)
    : base(repository, mapper)
        {
            _headersRepository = headersRepository ?? throw new ArgumentNullException(nameof(headersRepository));
            _requestSchemaRepository = requestSchemaRepository;
            _responseSchemaRepository = responseSchemaRepository;
            _reqResSchemaFieldsRepository = reqResSchemaFieldsRepository;
            _codeMapperRepository = codeMapperRepository;
            _languageSnippetRepository = languageSnippetRepository;
        }

        public async Task<List<ApiXVersionDto>> GetAllActiveAsync()
        {
            var entities = await _repository.GetAllActiveAsync();
            return _mapper.Map<List<ApiXVersionDto>>(entities);
        }

        public async Task<List<ApiXVersionDto>> GetByApiXCategoryUUIDAsync(string apiXCategoryUUID)
        {
            var entities = await _repository.FindAllAsync(x => x.ApiXCategoryUUID == apiXCategoryUUID && x.IsActive);
            return _mapper.Map<List<ApiXVersionDto>>(entities.OrderBy(x => x.SequenceNo));
        }

        /// <summary>
        /// Creates ApiXVersion record and related ApiXHeaders.
        /// Uses repositories only (no direct AppDbContext injection) to follow project pattern.
        /// Returns the generated Id of the created ApiXVersion.
        /// </summary>
        public async Task<int> CreateAsync(ApixVersionDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            // Map DTO to entity
            var entity = _mapper.Map<ApiXVersion>(dto);

            // Persist ApiXVersion via repository
            await _repository.AddAsync(entity);

            // If DTO contains headers (many DTO shapes attach a Headers collection), persist them via headers repository
            var headersProp = dto.GetType().GetProperty("Headers");
            if (headersProp != null)
            {
                var headersObj = headersProp.GetValue(dto) as IEnumerable<ApixHeaderDto>;
                if (headersObj != null)
                {
                    foreach (var h in headersObj)
                    {
                        var headerEntity = _mapper.Map<ApiXHeaders>(h);
                        // Link by UUID so it matches the existing schema (ApiXHeaders.ApiXVersionUUID)
                        headerEntity.ApiXVersionUUID = entity.UUID;
                        await _headersRepository.AddAsync(headerEntity);
                    }
                }
            }

            // Return numeric Id (matches earlier code patterns returning int Id)
            return Convert.ToInt32(entity.Id);
        }

        // Duplicate check: ensure same VersionNo under same ApiXCategoryUUID isn't repeated
        protected override async Task<bool> IsDuplicateAsync(ApixVersionCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.VersionNo!.ToLower().Trim() == (command.VersionNo ?? string.Empty).ToLower().Trim() &&
                x.ApiXCategoryUUID == command.ApiXCategoryUUID &&
                x.UUID != command.UUID);
        }

        // Search logic for datatables / paged results
        protected override Expression<Func<ApiXVersion, bool>>? BuildSearchFilter(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm)) return null;
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.VersionNo != null && x.VersionNo.ToLower().Contains(searchTerm)) ||
                (x.Title != null && x.Title.ToLower().Contains(searchTerm)) ||
                (x.ApiPath != null && x.ApiPath.ToLower().Contains(searchTerm));
        }

        // Sorting for datatables
        protected override Func<IQueryable<ApiXVersion>, IOrderedQueryable<ApiXVersion>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "versionno" => q => isAsc ? q.OrderBy(x => x.VersionNo) : q.OrderByDescending(x => x.VersionNo),
                "titile" => q => isAsc ? q.OrderBy(x => x.Title) : q.OrderByDescending(x => x.Title),
                "apipath" => q => isAsc ? q.OrderBy(x => x.ApiPath) : q.OrderByDescending(x => x.ApiPath),
                "sequenceno" => q => isAsc ? q.OrderBy(x => x.SequenceNo) : q.OrderByDescending(x => x.SequenceNo),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }

        public async Task<ApixVersionDto> SaveAndReturnAsync(ApixVersionCommand command, string userUuid, string ip)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            // Run validation and duplicate check (re-using base validation helpers)
            await ValidateAsync(command);

            if (await IsDuplicateAsync(command))
                throw new Exception($"{EntityDisplayName} already exists.");

            if (string.IsNullOrEmpty(command.UUID))
            {
                var entity = _mapper.Map<ApiXVersion>(command);
                entity.UUID = Utils.GetUUID();

                // Always create inactive
                entity.IsActive = false;

                OnBeforeCreate(entity, userUuid, ip);

                await _repository.AddAsync(entity);

                return _mapper.Map<ApixVersionDto>(entity);
            }
            else
            {
                // Update
                var entity = await _repository.GetByUuidAsync(command.UUID) ?? throw new Exception("Record not found");

                _mapper.Map(command, entity);

                OnBeforeUpdate(entity, userUuid, ip);

                await _repository.UpdateAsync(entity);

                return _mapper.Map<ApixVersionDto>(entity);
            }
        }

        public async Task<bool> CanActivateAsync(string versionUuid)
        {
            if (string.IsNullOrWhiteSpace(versionUuid))
                return false;

            // =========================
            // 1. HEADER CHECK
            // =========================
            var headers = await _headersRepository.GetAllActiveAsync();

            bool hasHeaders = headers.Any(h =>
                h.ApiXVersionUUID == versionUuid);

            if (!hasHeaders)
                return false;

            // =========================
            // 2. REQUEST SCHEMA + FIELD CHECK
            // =========================
            var requestSchemas = await _requestSchemaRepository.GetAllActiveAsync();
            var fields = await _reqResSchemaFieldsRepository.GetAllActiveAsync();

            var requestSchemaUuids = requestSchemas
                .Where(r => r.ApiXVersionUUID == versionUuid)
                .Select(r => r.UUID)
                .Where(u => !string.IsNullOrWhiteSpace(u))
                .ToList();

            bool hasRequestData =
                requestSchemaUuids.Any() &&
                fields.Any(f =>
                    !string.IsNullOrWhiteSpace(f.SchemaUUID) &&
                    requestSchemaUuids.Contains(f.SchemaUUID));

            if (!hasRequestData)
                return false;

            // =========================
            // 3. RESPONSE SCHEMAS (SUCCESS + FAILURE)
            // =========================
            var responseSchemas = await _responseSchemaRepository.GetAllActiveAsync();

            var successSchemaUuids = responseSchemas
                .Where(r =>
                    r.ApiXVersionUUID == versionUuid &&
                    r.IsSchemaForSuccess == true)
                .Select(r => r.UUID)
                .Where(u => !string.IsNullOrWhiteSpace(u))
                .ToList();

            var failureSchemaUuids = responseSchemas
                .Where(r =>
                    r.ApiXVersionUUID == versionUuid &&
                    r.IsSchemaForSuccess == false)
                .Select(r => r.UUID)
                .Where(u => !string.IsNullOrWhiteSpace(u))
                .ToList();

            bool hasSuccess =
                successSchemaUuids.Any() &&
                fields.Any(f =>
                    !string.IsNullOrWhiteSpace(f.SchemaUUID) &&
                    successSchemaUuids.Contains(f.SchemaUUID));

            bool hasFailure =
                failureSchemaUuids.Any() &&
                fields.Any(f =>
                    !string.IsNullOrWhiteSpace(f.SchemaUUID) &&
                    failureSchemaUuids.Contains(f.SchemaUUID));

            if (!hasSuccess || !hasFailure)
                return false;

            // =========================
            // 4. CODE MAPPER CHECK
            // =========================
            var mappers = await _codeMapperRepository.GetAllActiveAsync();

            bool hasMapper = mappers.Any(m =>
                m.ApiXVersionUUID == versionUuid);

            if (!hasMapper)
                return false;

            // =========================
            // 5. LANGUAGE SNIPPET CHECK
            // =========================
            var snippets = await _languageSnippetRepository.GetAllActiveAsync();

            bool hasSnippet = snippets.Any(s =>
                s.ApiXVersionUUID == versionUuid);

            if (!hasSnippet)
                return false;

            // =========================
            // SUCCESS
            // =========================
            return true;
        }

    }
}