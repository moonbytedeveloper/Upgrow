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
using VerifyIndia.Application.IServices.Menu;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.Master
{
    public class WLMasterPathPermissionService : MasterServiceBase<WL_Master_PathPermission, WLMasterPathPermissionDto, WLMasterPathPermissionCommand>, IWLMasterPathPermissionService
    {
        private readonly IMasterRepository<WL_Master_PathPermission> _pathPermissionRepository;
        private readonly IMasterRepository<WL_Master_Path> _pathRepository;
        private readonly IMenuRolePermissionService _menuRolePermissionService;

        public WLMasterPathPermissionService(
            IMasterRepository<WL_Master_PathPermission> repository,
            IMasterRepository<WL_Master_PathPermission> pathPermissionRepository,
            IMapper mapper,
            IMasterRepository<WL_Master_Path> pathRepository,
            IMenuRolePermissionService menuRolePermissionService)
            : base(repository, mapper)
        {
            _pathPermissionRepository = pathPermissionRepository;
            _pathRepository = pathRepository;
            _menuRolePermissionService = menuRolePermissionService;
        }

        protected override string EntityDisplayName => "Path";

        protected override async Task<bool> IsDuplicateAsync(WLMasterPathPermissionCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.PathUUID!.ToLower().Trim() == command.PathUUID.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        protected override Expression<Func<WL_Master_PathPermission, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                x.PathUUID != null && x.PathUUID.ToLower().Contains(searchTerm) ||
                x.PermissionUUID != null && x.PermissionUUID.ToLower().Contains(searchTerm);
        }

        protected override Func<IQueryable<WL_Master_PathPermission>, IOrderedQueryable<WL_Master_PathPermission>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "path" => q => isAsc ? q.OrderBy(x => x.PathUUID) : q.OrderByDescending(x => x.PathUUID),
                "permissionuuid" => q => isAsc ? q.OrderBy(x => x.PermissionUUID) : q.OrderByDescending(x => x.PermissionUUID),
                _ => q => q.OrderByDescending(x => x.Id)
            };
        }

        public async Task<List<WLMasterPathPermissionDto>> GetAllActiveAsync()
        {
            var entities = await _repository.GetAllActiveAsync();
            return _mapper.Map<List<WLMasterPathPermissionDto>>(entities);
        }

        public async Task<bool> HasAccessToPathAsync(string roleUuid, string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;

            var normalizedPath = path.TrimEnd('/').ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(normalizedPath))
                normalizedPath = "/";

            var allPaths = await _pathRepository.GetAllActiveAsync();
            var pathRecord = allPaths.FirstOrDefault(p =>
                !string.IsNullOrWhiteSpace(p.Path) &&
                p.Path.Trim().ToLowerInvariant() == normalizedPath);

            if (pathRecord == null)
                return true;

            var entries = await _pathPermissionRepository.GetAllActiveAsync();
            var matched = entries
                .Where(x => !string.IsNullOrWhiteSpace(x.PathUUID) &&
                            string.Equals(x.PathUUID.Trim(), pathRecord.UUID?.Trim(), StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!matched.Any())
                return true;

            if (matched.Any(x => string.IsNullOrWhiteSpace(x.PermissionUUID)))
                return true;

            if (string.IsNullOrWhiteSpace(roleUuid))
                return false;

            var rolePerms = await _menuRolePermissionService.GetAllActiveAsync();
            var roleHas = matched
                .Select(m => m.PermissionUUID?.Trim())
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .Any(p => rolePerms.Any(rp =>
                    string.Equals(rp.PermissionUUID?.Trim(), p, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(rp.RoleUUID?.Trim(), roleUuid.Trim(), StringComparison.OrdinalIgnoreCase) &&
                    rp.IsActive));

            return roleHas;
        }
    }
}
