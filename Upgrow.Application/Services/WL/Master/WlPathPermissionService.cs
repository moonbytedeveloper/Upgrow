using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.IServices.Menu;
using Upgrow.Application.IServices.WL.Master;
using Upgrow.Domain.Entities;
using Upgrow.Domain.Entities.WL.Master;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.Services.WL.Master
{
    public class WlPathPermissionService : IWlPathPermissionService
    {
        private readonly IMasterRepository<WL_Master_Path> _pathRepository;
        private readonly IMasterRepository<WL_Master_PathPermission> _pathPermissionRepository;
        private readonly IWLMenuRolePermissionService _menuRolePermissionService;
        private readonly ILogger<WlPathPermissionService> _logger;

        public WlPathPermissionService(
            IMasterRepository<WL_Master_Path> pathRepository,
            IMasterRepository<WL_Master_PathPermission> pathPermissionRepository,
            IWLMenuRolePermissionService menuRolePermissionService,
            ILogger<WlPathPermissionService> logger)
        {
            _pathRepository = pathRepository;
            _pathPermissionRepository = pathPermissionRepository;
            _menuRolePermissionService = menuRolePermissionService;
            _logger = logger;
        }

        public async Task<bool> HasAccessToPathAsync(string roleUuid, string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;

            // Normalize path
            var normalizedPath = path.TrimEnd('/').ToLowerInvariant();
            if (string.IsNullOrEmpty(normalizedPath))
                normalizedPath = "/";

            // Step 1: Find the path record by matching the path string
            var allPaths = await _pathRepository.GetAllActiveAsync();
            var pathRecord = allPaths.FirstOrDefault(p =>
                !string.IsNullOrWhiteSpace(p.Path) &&
                p.Path.Trim().ToLowerInvariant() == normalizedPath);

            // If path not found -> treat as public (allow)
            if (pathRecord == null)
            {
                _logger.LogWarning("Path {Path} not configured, allowing access", path);
                return true;
            }

            // Step 2: Get all active path-permission entries for this path UUID
            var allPathPermissions = await _pathPermissionRepository.GetAllActiveAsync();
            var pathPermissions = allPathPermissions
                .Where(x => !string.IsNullOrWhiteSpace(x.PathUUID) &&
                           x.PathUUID.Trim() == pathRecord.UUID.Trim())
                .ToList();

            // If no configuration exists for this path -> allow (public)
            if (!pathPermissions.Any())
            {
                _logger.LogWarning("No path permission configured for path UUID {PathUUID}, allowing access", pathRecord.UUID);
                return true;
            }

            // Step 3: Check if any permission entry doesn't require a specific permission (public access)
            if (pathPermissions.Any(x => string.IsNullOrWhiteSpace(x.PermissionUUID)))
            {
                _logger.LogWarning("Public access configured for path {Path}, allowing access", path);
                return true;
            }

            // Step 4: If role is not provided -> deny
            if (string.IsNullOrWhiteSpace(roleUuid))
            {
                _logger.LogWarning("No role UUID provided for restricted path {Path}, denying access", path);
                return false;
            }

            // Step 5: Check if the role has permission to access this path
            // Get required permission UUIDs for this path
            var requiredPermissions = pathPermissions
                .Select(x => x.PermissionUUID?.Trim())
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .Distinct()
                .ToList();

            // Get role->permission mappings for this role
            var allMenuRolePermissions = await _menuRolePermissionService.GetAllActiveAsync();
            var rolePermissions = allMenuRolePermissions
                .Where(rp => rp.RoleUUID.Trim() == roleUuid.Trim())
                .ToList();

            // Check if role has ANY of the required permissions
            var roleHasPermission = requiredPermissions.Any(requiredPerm =>
                rolePermissions.Any(rp =>
                    rp.PermissionUUID.Trim() == requiredPerm &&
                    rp.RoleUUID.Trim() == roleUuid.Trim() &&
                    rp.IsActive));

            if (roleHasPermission)
            {
                _logger.LogInformation("Role {RoleUUID} has permission for path {Path}", roleUuid, path);
                return true;
            }

            _logger.LogWarning("Role {RoleUUID} denied access to path {Path}", roleUuid, path);
            return false;
        }
    }
}