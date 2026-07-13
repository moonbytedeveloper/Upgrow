using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moonbyte.UI;
using System;
using Upgrow.Application;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.Commands.Menu;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.DTO.Menu;
using Upgrow.Application.IServices;
using Upgrow.Application.IServices.Master;
using Upgrow.Application.IServices.Menu;
using Upgrow.Application.Services.Menu;
using Upgrow.Infrastructure.Filters;
using Upgrow.Infrastructure.Services;
using UpgrowAdminPanel.Models.Menu;

namespace UpgrowAdminPanel.Controllers
{
    [ActivityLog]
    public class MenuController : BaseController
    {

        private readonly IMenuRolePermissionService _menurolePermissionService;
        private readonly IMasterRoleService _menuRoleService;
        private readonly IMasterPermissionService _permissionService;
        private readonly IMenuQueryService _menuQueryService;
        private readonly IMasterPermissionGroupService _permissionGroupService;
        private readonly IDomainResolverService _tenantDomainResolver;
        public MenuController(
            IEncryptionService encryptionService,
            IMenuRolePermissionService menurolePermissionService,
            IDataTableParser dataTableParser,
            IMasterRoleService roleService,
            IMasterPermissionService permissionService,
            IMasterPermissionGroupService permissionGroupService,
            IMenuQueryService menuQueryService,
            IDomainResolverService tenantDomainResolver) : base(dataTableParser, tenantDomainResolver, encryptionService)
        {
            _menurolePermissionService = menurolePermissionService;
            _menuRoleService = roleService;
            _permissionService = permissionService;
            _menuQueryService = menuQueryService;
            _permissionGroupService = permissionGroupService;
            _tenantDomainResolver = tenantDomainResolver;
        }

        #region Menu Role Permission
        // Developed by Krishna
        // Date : 23-03-2026



        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Menu Role Permission List", MenuName = "Menu_RolePermission")]
        public IActionResult MenuViewRolePermission() => View();


        [HttpPost]
        public Task<IActionResult> GetMenuRolePermission()
        => GetPagedDataAsync<MenuRolePermissionDto, MenuRolePermissionCommand>(_menurolePermissionService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["role"] = dto.RoleUUID,
            ["permission"] = dto.PermissionUUID,
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleRolePermission), "menupermission")
        });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Menu Role Permission", MenuName = "Menu_RolePermission")]
        public async Task<IActionResult> MenuAddRolePermission()
        {
            // Load email credentials for dropdown
            var roles = await _menuRoleService.GetDropdownAsync(x => x.Title);
            var permission = await _permissionService.GetDropdownAsync(x => x.Name);
            var selectItems = roles.Select(x => new SelectListItem
            {
                Value = x.UUID,
                Text = x.Title,
            }).ToList();
            var selectPermission = permission.Select(x => new SelectListItem
            {
                Value = x.UUID,
                Text = x.Title,
            }).ToList();
            var vm = new MenuRolePermissionVM
            {
                RoleList = selectItems,
                PermissionList = selectPermission
            };
           
            vm.RolePermission = new MenuRolePermissionCommand { IsActive = true };
            return View("MenuAddRolePermission", vm);
            
        }


        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Menu Role Permission", MenuName = "Menu_RolePermission")]
        public async Task<IActionResult> MenuEditRolePermission(string? uuid)
        {
            // Load email credentials for dropdown
            var roles = await _menuRoleService.GetDropdownAsync(x => x.Title);
            var permission = await _permissionService.GetDropdownAsync(x => x.Name);
            var selectItems = roles.Select(x => new SelectListItem
            {
                Value = x.UUID,
                Text = x.Title,
            }).ToList();
            var selectPermission = permission.Select(x => new SelectListItem
            {
                Value = x.UUID,
                Text = x.Title,
            }).ToList();
            var vm = new MenuRolePermissionVM
            {
                RoleList = selectItems,
                PermissionList = selectPermission
            };

            if (string.IsNullOrEmpty(uuid))
            {
                vm.RolePermission = new MenuRolePermissionCommand { IsActive = true };
                return View("MenuAddRolePermission", vm);
            }

            var dto = await _menurolePermissionService.GetByUuidAsync(uuid);

            if (dto == null)
            {
                SetErrorMessage("Record not found!");
                return RedirectToAction(nameof(MenuViewRolePermission));
            }

            vm.RolePermission = new MenuRolePermissionCommand
            {
                UUID = dto.UUID,
                RoleUUID = dto.RoleUUID,
                PermissionUUID = dto.PermissionUUID,
                IsActive = dto.IsActive
            };

            return View("MenuAddRolePermission", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Menu Role Permission Form", MenuName = "Menu_RolePermission")]
        public async Task<IActionResult> MenuAddRolePermission(MenuRolePermissionVM vm)
        {
            if (vm == null)
                return BadRequest();

            // Validate inner command model
            if (!TryValidateModel(vm.RolePermission, nameof(vm.RolePermission)))
            {
                // repopulate dropdown on validation failure
                var roles = await _menuRoleService.GetDropdownAsync(x => x.Title);
                var permission = await _permissionService.GetDropdownAsync(x => x.Name);
                var selectItems = roles.Select(x => new SelectListItem
                {
                    Value = x.UUID,
                    Text = x.Title,
                }).ToList();
                var selectPermission = permission.Select(x => new SelectListItem
                {
                    Value = x.UUID,
                    Text = x.Title,
                }).ToList();
                return View("MenuAddRolePermission", vm);
            }

            try
            {
                await _menurolePermissionService.SaveAsync(vm.RolePermission, GetUserUUID(), Utils.GetLocalIPAddress());
                _menuQueryService?.InvalidateCache();
                SetSuccessMessage(string.IsNullOrEmpty(vm.RolePermission.UUID)
                    ? "Role Permission added successfully!"
                    : "Role Permission updated successfully!");

                return RedirectToAction(nameof(MenuViewRolePermission));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);

                // Reload dropdown on error
                var roles = await _menuRoleService.GetDropdownAsync(x => x.UUID);
                var permission = await _permissionService.GetDropdownAsync(x => x.Name);
                vm.RoleList = roles.Select(x => new SelectListItem
                {
                    Value = x.UUID,
                    Text = x.Title
                }).ToList();
                var selectPermission = permission.Select(x => new SelectListItem
                {
                    Value = x.UUID,
                    Text = x.Title,
                }).ToList();
                return View("MenuAddRolePermission", vm);
            }
        }


        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Menu Role Permission Status", MenuName = "Menu_RolePermission")]
        public async Task<IActionResult> ToggleRolePermission(string uuid)
        {
            try
            {
                var newStatus = await _menurolePermissionService.ToggleActiveAsync(uuid, GetUserUUID(), Utils.GetLocalIPAddress());

                // invalidate cached menu rights so header updates immediately
                _menuQueryService?.InvalidateCache();

                return Json(new
                {
                    success = true,
                    isActive = newStatus,
                    message = "Role Permission status updated successfully!"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // [HttpPost]
        //public Task<IActionResult> ToggleRolePermission(string uuid)
        //    => ToggleActiveAsync<MasterRolePermissionDto, MasterRolePermissionCommand>(uuid, _rolePermissionService, "Role Permission");
        #endregion

        #region Menu Role Permission Final
        //developed by Utsav
        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Menu Role Permission Management", MenuName = "Menu_RolePermission")]
        public async Task<IActionResult> ManageMenuRolePermission()
        {
            try
            {
                var vm = new MenuRolePermissionVM
                {
                    RoleList = (await _menuRoleService.GetDropdownAsync(x => x.Title))
                        .Select(x => new SelectListItem { Value = x.UUID, Text = x.Title })
                        .ToList()

                };

                return View("ManageMenuRolePermission", vm);
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                return View("ManageMenuRolePermission", new MenuRolePermissionVM());
            }
        }

        [HttpPost]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Fetch Permissions For Role", MenuName = "Menu_RolePermission")]
        public async Task<IActionResult> GetPermissionsByRole(string roleUUID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(roleUUID))
                    return Json(new { success = false, message = "Role ID is required." });

                // Get all permission groups
                var allGroups = await _permissionGroupService.GetDropdownAsync(x => x.Title);

                // Get all permissions
                var allPermissions = await _permissionService.GetAllActiveAsync();

                // ✅ Get ONLY ACTIVE permissions for this role (NOT old inactive ones)
                var activeRolePermissions = await _menurolePermissionService.GetAllByRoleAsync(roleUUID);

                // ✅ Filter to only permissions that have a permission group UUID
                // (This excludes old permissions with no group assigned)
                var displayedPermissions = activeRolePermissions
                    .Where(rp => allPermissions.Any(p => p.UUID == rp.PermissionUUID && !string.IsNullOrWhiteSpace(p.PermissionGroupUUID)))
                    .ToList();

                // Get permission UUIDs that will be displayed in the form
                var assignedPermissionUUIDs = displayedPermissions
                    .Select(p => p.PermissionUUID)
                    .ToList();

                // Build response - group permissions by their permission group
                var permissionGroups = allGroups
                    .OrderBy(g => g.Title)
                    .Select(group => new PermissionGroupWithPermissionsDto
                    {
                        GroupUUID = group.UUID,
                        GroupTitle = group.Title,
                        Permissions = allPermissions
                            .Where(p => p.PermissionGroupUUID == group.UUID)
                            .OrderBy(p => p.Name)
                            .Select(p => new PermissionWithStatusDto
                            {
                                PermissionUUID = p.UUID,
                                PermissionName = p.Name,
                                PermissionDescription = p.Description,
                                IsAssigned = assignedPermissionUUIDs.Contains(p.UUID)
                            })
                            .ToList()
                    })
                    .Where(g => g.Permissions.Count > 0)
                    .ToList();

                return Json(new { success = true, permissionGroups = permissionGroups });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Save Permissions For Role", MenuName = "Menu_RolePermission")]
        public async Task<IActionResult> SaveMenuRolePermissions()
        {
            try
            {
                var roleUUID = Request.Form["roleUUID"].ToString();

                if (string.IsNullOrWhiteSpace(roleUUID))
                    return Json(new { success = false, message = "Role ID is required." });

                // Extract ALL permissions from form (checked and unchecked)
                var selectedPermissions = new List<string>();
                var displayedPermissions = new List<string>();

                foreach (var key in Request.Form.Keys)
                {
                    // Look for patterns like: permissions[0].permissionUUID
                    var match = System.Text.RegularExpressions.Regex.Match(key, @"permissions\[(\d+)\]\.permissionUUID");

                    if (match.Success)
                    {
                        int index = int.Parse(match.Groups[1].Value);
                        var isActiveKey = $"permissions[{index}].isActive";
                        var permissionUUIDValue = Request.Form[key].ToString();

                        if (!string.IsNullOrWhiteSpace(permissionUUIDValue))
                        {
                            // ✅ Track ALL displayed permissions
                            displayedPermissions.Add(permissionUUIDValue);

                            // Only add if isActive is true (checkbox is checked)
                            if (Request.Form.ContainsKey(isActiveKey) &&
                                bool.TryParse(Request.Form[isActiveKey].ToString(), out bool isActive) &&
                                isActive)
                            {
                                selectedPermissions.Add(permissionUUIDValue);
                            }
                        }
                    }
                }

                // ✅ Pass BOTH selected AND displayed permissions to service
                await _menurolePermissionService.SaveSelectedPermissionsAsync(
                    roleUUID,
                    selectedPermissions,
                    displayedPermissions,  // ← NEW: Pass displayed permissions
                    GetUserUUID(),
                    Utils.GetLocalIPAddress());

                _menuQueryService?.InvalidateCache();
              //  SetSuccessMessage("Permissions saved successfully!");
                return Json(new { success = true, message = "Permissions saved successfully!" });
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }
        #endregion
    }
}