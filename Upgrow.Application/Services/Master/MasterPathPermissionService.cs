using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.IServices.Master;
using Upgrow.Application.IServices.Menu;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.Services.Master
{
    public class MasterPathPermissionService : MasterServiceBase<Master_PathPermission, MasterPathPermissionDto, MasterPathPermissionCommand>, IMasterPathPermissionService
    {
        private readonly IMasterRepository<Master_PathPermission> _pathPermissionRepository;
        private readonly IMasterRepository<Master_Path> _pathRepository; // <-- added
        private readonly IMenuRolePermissionService _menuRolePermissionService; // <-- added

        public MasterPathPermissionService(IMasterRepository<Master_PathPermission> repository, 
            IMasterRepository<Master_PathPermission> pathPermissionRepository, 
            IMapper mapper,
            IMasterRepository<Master_Path> pathRepository,                          // <-- added
            IMenuRolePermissionService menuRolePermissionService)
           : base(repository, mapper) 
        {
            _pathPermissionRepository = pathPermissionRepository;
            _pathRepository = pathRepository;
            _menuRolePermissionService = menuRolePermissionService;
        }

        protected override string EntityDisplayName => "Path";

        protected override async Task<bool> IsDuplicateAsync(MasterPathPermissionCommand command)
        {
            return await _repository.ExistsAsync(x =>
            x.PathUUID!.ToLower().Trim() == command.PathUUID.ToLower().Trim() &&
            x.UUID != command.UUID);
        }

        protected override Expression<Func<Master_PathPermission, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                x.PathUUID != null && x.PathUUID.ToLower().Contains(searchTerm) ||
                x.PermissionUUID != null && x.PermissionUUID.ToLower().Contains(searchTerm);
        }

        protected override Func<IQueryable<Master_PathPermission>, IOrderedQueryable<Master_PathPermission>>? BuildSortExpression(
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

        public async Task<List<MasterPathPermissionDto>> GetAllActiveAsync()
        {
            var entities = await _repository.GetAllActiveAsync();
            return _mapper.Map<List<MasterPathPermissionDto>>(entities);
        }
        public async Task<bool> HasAccessToPathAsync(string roleUuid, string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;

            var normalizedPath = path.TrimEnd('/').ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(normalizedPath))
                normalizedPath = "/";

            // Find path record (by normalized path string)
            var allPaths = await _pathRepository.GetAllActiveAsync();
            var pathRecord = allPaths.FirstOrDefault(p =>
                !string.IsNullOrWhiteSpace(p.Path) &&
                p.Path.Trim().ToLowerInvariant() == normalizedPath);

            // If path not configured -> treat as public (allow)
            if (pathRecord == null)
                return true;

            // Load all active path-permission entries for that path UUID
            var entries = await _pathPermissionRepository.GetAllActiveAsync();
            var matched = entries
                .Where(x => !string.IsNullOrWhiteSpace(x.PathUUID) &&
                            string.Equals(x.PathUUID.Trim(), pathRecord.UUID?.Trim(), StringComparison.OrdinalIgnoreCase))
                .ToList();

            // If no configuration exists for this path -> allow (public)
            if (!matched.Any())
                return true;

            // If any matched entry does not require a permission -> allow
            if (matched.Any(x => string.IsNullOrWhiteSpace(x.PermissionUUID)))
                return true;

            // If role is not provided -> deny
            if (string.IsNullOrWhiteSpace(roleUuid))
                return false;

            // Get role->permission mappings and check if any permission for this path is granted to the role
            var rolePerms = await _menuRolePermissionService.GetAllActiveAsync();
            var roleHas = matched
                .Select(m => m.PermissionUUID?.Trim())
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .Any(p => rolePerms.Any(rp =>
                    string.Equals(rp.PermissionUUID?.Trim(), p, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(rp.RoleUUID?.Trim(), roleUuid.Trim(), StringComparison.OrdinalIgnoreCase) &&
                    rp.IsActive));

            if (roleHas)
                return true;

            // Deny by default
            return false;
        }


    }
}

   
