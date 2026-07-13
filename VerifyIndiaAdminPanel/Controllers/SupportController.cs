using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using VerifyIndia.Application;
using VerifyIndia.Application.Commands.Support;
using VerifyIndia.Application.DTO.Support;
using VerifyIndia.Application.IServices;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Application.IServices.Support;
using VerifyIndia.Application.IServices.WL;
using VerifyIndia.Application.Services;
using VerifyIndia.Infrastructure.Filters;
using VerifyIndiaAdminPanel.Models.Master;
using VerifyIndiaAdminPanel.Models.Support;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Net.WebRequestMethods;

namespace VerifyIndiaAdminPanel.Controllers
{
    [ActivityLog]
    public class SupportController : BaseController
    {
        private readonly IDataTableParser _dataTableParser;
        private readonly ISupportTicketCategoryService _ticketCategoryService;
        private readonly ISupportTicketHeaderService _ticketHeaderService;
        private readonly IMasterDesignationService _designationService;
        private readonly IMasterDepartmentService _departmentService;
        private readonly IDomainResolverService _tenantDomainResolver;
        private readonly IFileUploadService _fileUploadService;
        private readonly IMasterEmployeeService _employeeService;
        private readonly IDomainResolverService _domainResolverService;
        private readonly IWLTenantService _tenantService;
        private readonly IMasterCustomerService _customerService;
        private readonly IMasterNomenclatureService _nomenclatureService;

        public SupportController(
            IEncryptionService encryptionService,
            IDataTableParser dataTableParser, 
            ISupportTicketCategoryService ticketCategoryService,
            ISupportTicketHeaderService ticketHeaderService,
            IMasterDesignationService designationService,
            IDomainResolverService tenantDomainResolver,
            IFileUploadService fileUploadService,
            IMasterDepartmentService departmentService,
            IMasterEmployeeService employeeService,
            IDomainResolverService domainResolverService,
            IWLTenantService tenantService,
            IMasterCustomerService customerService,
            IMasterNomenclatureService nomenclatureService
            ) : base(dataTableParser, tenantDomainResolver, encryptionService)
        {
            _dataTableParser = dataTableParser;
            _ticketCategoryService = ticketCategoryService;
            _ticketHeaderService = ticketHeaderService;
            _designationService = designationService;
            _tenantDomainResolver = tenantDomainResolver;
            _fileUploadService = fileUploadService;
            _departmentService = departmentService;
            _employeeService = employeeService;
            _domainResolverService = domainResolverService;
            _tenantService = tenantService;
            _customerService = customerService;
            _nomenclatureService = nomenclatureService;
        }
        #region Helper Method
        private async Task<SupportTicketDetailsVM?> BuildTicketDetailsVM(string ticketUuid)
        {
            if (string.IsNullOrWhiteSpace(ticketUuid))
                return null;

            var ticket = await _ticketHeaderService.GetByUuidAsync(ticketUuid);
            if (ticket == null)
                return null;

            var messages = await _ticketHeaderService.GetTicketConversationAsync(ticketUuid);
            var employee = await _employeeService.GetByUuidAsync(ticket.AssigneeUUID);

            var messageVMs = new List<TicketMessageVM>();

            foreach (var x in messages)
            {
                var urlsWithDomain = new List<string>();

                if (x.Url != null)
                {
                    foreach (var url in x.Url)
                    {
                        var fullUrl = _domainResolverService.BuildAbsoluteUrl(url);
                        urlsWithDomain.Add(fullUrl);
                    }
                }

                messageVMs.Add(new TicketMessageVM
                {
                    LineUUID = x.LineUUID,
                    UserName = x.UserName,
                    UserType = x.UserType,
                    Message = x.Message,
                    CreatedAt = x.CreatedAt?.ToLocalTime(),
                    Url = x.Url,
                    UrlWithDomain = urlsWithDomain
                });
            }

            return new SupportTicketDetailsVM
            {
                TicketUUID = ticket.UUID,
                TicketNumber = ticket.TicketNumber,
                Subject = ticket.Subject,
                CreatedAt = ticket.CreatedAt.ToLocalTime(),
                UpdatedOn = ticket.UpdateOn,
                Assignee = employee != null ? employee.FirstName + " " + employee.LastName : "",
                Status = ticket.IsActive ? "Open" : "Closed",
                Messages = messageVMs
            };
        }
        private async Task<bool> UpdateTicketStatusAsync(string ticketUuid, bool isActive, string? otp = null)
        {
            if (string.IsNullOrWhiteSpace(ticketUuid))
                return false;

            var ticket = await _ticketHeaderService.GetByUuidAsync(ticketUuid);
            if (ticket == null)
                return false;

            if (!isActive)
            {
                if (string.IsNullOrWhiteSpace(ticket.Otp) || string.IsNullOrWhiteSpace(otp))
                    return false;

                if (!string.Equals(ticket.Otp.Trim(), otp.Trim(), StringComparison.Ordinal))
                    return false;
            }

            var command = new SupportTicketHeaderCommand
            {
                UUID = ticket.UUID,
                TicketCategoryUUID = ticket.TicketCategoryUUID,
                UserType = ticket.UserType,
                AssigneeUUID = ticket.AssigneeUUID,
                UserUUID = ticket.UserUUID,
                CreatedAt = ticket.CreatedAt,
                TicketNumber = ticket.TicketNumber,
                Subject = ticket.Subject,
                IsActive = isActive,
                Otp = isActive ? null : ticket.Otp
            };

            await _ticketHeaderService.SaveAsync(
                command,
                GetUserUUID(),
                Utils.GetLocalIPAddress()
            );

            return true;
        }
        private async Task<string?> GetCompanyNameAsync()
        {
            try
            {
                // Prefer tenant identifier resolved from request host (already stored sanitized in DB)
                var tenantFromHost = await ResolveCompanyNameFromRequestAsync();
                if (!string.IsNullOrWhiteSpace(tenantFromHost))
                    return tenantFromHost.Trim();

                return null; // FileUploadService will fallback to "Unknown"
            }
            catch
            {
                return null;
            }
        }
        public List<SelectListItem> GetUserTypeList()
        {
            var userTypes = new List<SelectListItem>
        {
        new SelectListItem { Text = "White Label", Value = "WhiteLabel" },
        new SelectListItem { Text = "Customer", Value = "Customer" },
        //new SelectListItem { Text = "Distributor", Value = "Distributor" },
        new SelectListItem { Text = "Agent", Value = "Agent" }
        };

            return userTypes;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersByType(string userType)
        {
            var data = await GetUsersByTypeInternal(userType);
            return Json(data);
        }
        [HttpGet]
        public async Task<IActionResult> GetUsersAndCategories(string userType)
        {
            userType = userType?.Trim().ToLower();
            var users = await GetUsersByTypeInternal(userType);
            var categories = await _ticketCategoryService.GetAllAsync(x =>
                x.UserType.ToLower() == userType
            );
            var categoryList = categories.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.UUID
            }).ToList();
            return Json(new
            {
                users,
                categories = categoryList
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetCategoryByUserType(string userType)
        {
            if (string.IsNullOrWhiteSpace(userType))
                return Json(new List<SelectListItem>());

            userType = userType.Trim().ToLower();

            var categories = await _ticketCategoryService.GetAllAsync(x =>
                x.UserType.ToLower() == userType && x.IsActive);

            var result = categories.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.UUID
            }).ToList();

            return Json(result);
        }
        private async Task LoadDropdowns(SupportTicketsVM vm)
        {
            vm.CategoryList = new List<SelectListItem>();
            vm.UserList = new List<SelectListItem>();
        }
        private async Task<List<SelectListItem>> GetUsersByTypeInternal(string userType)
        {
            userType = userType?.Trim().ToLower();

            switch (userType)
            {
                case "whitelabel":
                    var tenants = await _tenantService.GetAllAsync();
                    return tenants.Select(x => new SelectListItem
                    {
                        Text = x.TenantName,
                        Value = x.UUID
                    }).ToList();

                case "agent":
                    var agents = await _customerService.GetAllAsync(x => x.IsAgent == true);
                    return agents.Select(x => new SelectListItem
                    {
                        Text = ((x.FName ?? "") + " " + (x.LName ?? "")).Trim(),
                        Value = x.UUID
                    }).ToList();

                case "customer":
                    var customers = await _customerService.GetAllAsync(x => x.IsAgent == false);
                    return customers.Select(x => new SelectListItem
                    {
                        Text = ((x.FName ?? "") + " " + (x.LName ?? "")).Trim(),
                        Value = x.UUID
                    }).ToList();

                default:
                    return new List<SelectListItem>();
            }
        }
        #endregion


        #region Ticket Category
        // Developed By : Hardik (07-04-2026)

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Support Ticket Categories List", MenuName = "Support_TicketCategory")]
        public IActionResult ViewSTCategory() => View();

        [HttpPost]
        public Task<IActionResult> GetSTCategory()
        => GetPagedDataAsync<SupportTicketCategoryDto, SupportTicketCategoryCommand>(_ticketCategoryService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["title"] = dto.Title,
            ["designation"] = dto.DesignationUUID,
            ["usertype"] = dto.UserType,
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleSTCategory), "Ticket Category")
        });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Support Ticket Category Page", MenuName = "Support_TicketCategory")]
        public async Task<IActionResult> AddSTCategory()
        {
            var vm = new SupportTicketCategoryVM
            {
                DesignationList = await LoadDropdownAsync(_designationService, x => x.Title),
                STC = new SupportTicketCategoryCommand { IsActive = true },
                UserTypeList = GetUserTypeList()
            };

            return View(vm);
        }

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Support Ticket Category Page", MenuName = "Support_TicketCategory")]
        public Task<IActionResult> EditSTCategory(string? uuid)
        => EditMasterAsync<SupportTicketCategoryDto, SupportTicketCategoryCommand, SupportTicketCategoryVM>(
            uuid,
            _ticketCategoryService,
            () => new SupportTicketCategoryCommand { IsActive = true },
            dto => new SupportTicketCategoryCommand
            {
                UUID = dto.UUID,
                Title = dto.Title,
                DesignationUUID = dto.DesignationUUID,
                UserType = dto.UserType,
                IsActive = dto.IsActive
            },
            async command => new SupportTicketCategoryVM
            {
                DesignationList = await LoadDropdownAsync(_designationService, x => x.Title),
                STC = command,
                UserTypeList = GetUserTypeList()
            },
            nameof(AddSTCategory),
            nameof(ViewSTCategory)
        );

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Support Ticket Category Form", MenuName = "Support_TicketCategory")]
        public Task<IActionResult> AddSTCategory(SupportTicketCategoryVM vm)
        => SaveMasterAsync<SupportTicketCategoryDto, SupportTicketCategoryCommand, SupportTicketCategoryVM>(
            vm,
            x => x.STC,
            async x =>
            {
                x.DesignationList = await LoadDropdownAsync(_designationService, d => d.Title);
                x.UserTypeList = GetUserTypeList();
                return x;
            },
            _ticketCategoryService,
            "Support ticket category",
            nameof(AddSTCategory),
            nameof(ViewSTCategory),
            nameof(SupportTicketCategoryVM.STC));

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggle Support Ticket Category Status", MenuName = "Support_TicketCategory")]
        public Task<IActionResult> ToggleSTCategory(string uuid)
            => ToggleActiveAsync<SupportTicketCategoryDto, SupportTicketCategoryCommand>(uuid, _ticketCategoryService, "Support Ticket Category");
        #endregion

        #region Support Tickets

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Support Tickets List", MenuName = "Support_TicketHeader")]
        public IActionResult ViewSupportTickets()
        {
            var vm = new SupportTicketsVM();
            return View(vm);
        }

        [HttpPost]
        public Task<IActionResult> GetSupportTickets(string? userType, string? departmentUuid, string? assigneeUuid)
    => GetPagedDataAsync(
        request => _ticketHeaderService.GetPagedAsync(request, userType, departmentUuid, assigneeUuid),
        dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["ticketnumber"] = dto.TicketNumber,
            ["raisedby"] = BuildRaisedByHtml(dto),
            ["category"] = dto.TicketCategoryUUID,
            ["subject"] = dto.Subject,
            ["message"] = dto.Message,
            ["createdat"] = ToUserTimeString(dto.CreatedAt),
            ["assignee"] = string.IsNullOrWhiteSpace(dto.AssigneeUUID) ? "-" : dto.AssigneeUUID,
            ["status"] = dto.IsActive
                ? "<span class='badge badge-outline-danger'>Open</span>"
                : "<span class='badge badge-outline-success'>Closed</span>"
        });

        private static string BuildRaisedByHtml(SupportTicketDto dto)
        {
            var name = string.IsNullOrWhiteSpace(dto.RaisedByName) ? "-" : dto.RaisedByName.Trim();
            var mobile = string.IsNullOrWhiteSpace(dto.RaisedByMobile) ? "-" : dto.RaisedByMobile.Trim();
            var email = string.IsNullOrWhiteSpace(dto.RaisedByEmail) ? "-" : dto.RaisedByEmail.Trim();

            return "<ul class='mb-0 ps-3'>" +
                   $"<li><b>Name :</b> {WebUtility.HtmlEncode(name)}</li>" +
                   $"<li><b>Mob. No. :</b> {WebUtility.HtmlEncode(mobile)}</li>" +
                   $"<li><b>Email Id :</b> {WebUtility.HtmlEncode(email)}</li>" +
                   "</ul>";
        }

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Get Assignable Employees For Ticket", MenuName = "Support_Tickets")]
        public async Task<IActionResult> GetAssignableEmployees(string ticketUuid)
        {
            if (string.IsNullOrWhiteSpace(ticketUuid))
                return Json(new { success = false, message = "Invalid ticket." });

            var ticket = await _ticketHeaderService.GetByUuidAsync(ticketUuid);
            if (ticket == null)
                return Json(new { success = false, message = "Ticket not found." });

            var data = await _ticketHeaderService.GetAssignableEmployeesAsync(ticket.TicketCategoryUUID);
            return Json(new
            {
                success = true,
                data,
                selectedAssigneeUuid = ticket.AssigneeUUID
            });
        }
        [HttpGet]
        public async Task<IActionResult> AddSupportTicket()
        {
            var vm = new SupportTicketsVM
            {
                SupportTicket = new SupportTicketHeaderCommand
                {
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow

                }
            };

            await LoadDropdowns(vm);

            return View("AddSupportTicket", vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSupportTicket(SupportTicketsVM vm)
        {
            if (vm?.SupportTicket == null)
                return BadRequest();
            vm.SupportTicket.TicketNumber = await _nomenclatureService.GenerateCodeAsync("SupportTicket");
            if (string.IsNullOrEmpty(vm.SupportTicket.TicketNumber))
            {
                ModelState.AddModelError("", "SupportTicket nomenclature is not configured. Please configure it first.");
                await LoadDropdowns(vm);
                return View("AddSupportTicket", vm);
            }
            //if (!TryValidateModel(vm.SupportTicket, "SupportTicket"))
            //{
            //    await LoadDropdowns(vm);
            //    return View(vm);
            //}

            try
            {
                var attachmentPaths = new List<string>();
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf" };

                if (vm.SupportTicketAttachment?.Image != null)
                {
                    foreach (var file in vm.SupportTicketAttachment.Image)
                    {
                        var path = await _fileUploadService.SaveFileAsync(
                            file,
                            await GetCompanyNameAsync(),
                            "SupportTickets",
                            allowedExtensions);

                        attachmentPaths.Add(path);
                    }
                }

                await _ticketHeaderService.SaveAsync(
                    vm.SupportTicket,
                    vm.SupportTicketLine?.Message,
                    attachmentPaths,
                    GetUserUUID(),
                    Utils.GetLocalIPAddress()
                );

                SetSuccessMessage("Ticket created successfully!");
                return RedirectToAction(nameof(ViewSupportTickets));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                await LoadDropdowns(vm);
                return View(vm);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Update", Description = "Assigned Ticket To Employee", MenuName = "Support_TicketHeader")]
        public async Task<IActionResult> AssignTicket(string ticketUuid, string assigneeUuid)
        {
            try
            {
                await _ticketHeaderService.AssignTicketAsync(ticketUuid, assigneeUuid, GetUserUUID(), Utils.GetLocalIPAddress());
                return Json(new { success = true, message = "Ticket assigned successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        
        #endregion

        #region Open Tickets
        [HttpGet]

        public IActionResult ViewOpenTickets()
        {
            var vm = new SupportTicketsVM
            {
                UserTypeList = GetUserTypeList()
            };

            return View(vm);
        }

        [HttpPost]
        public Task<IActionResult> GetOpenTickets(string? userType, string? departmentUuid, string? assigneeUuid)
        {
            var currentUserUuid = GetUserUUID();

            return GetPagedDataAsync(
                request => _ticketHeaderService.GetPagedAsync(
                    request,
                    userType,
                    departmentUuid,
                    assigneeUuid,
                    true,
                    currentUserUuid 
                ),
                dto => new Dictionary<string, object>
                {
                    ["uuid"] = dto.UUID,
                    ["ticketnumber"] = dto.TicketNumber,
                    ["raisedby"] = BuildRaisedByHtml(dto),
                    ["subject"] = dto.Subject,
                    ["message"] = dto.Message,
                    ["createdat"] = ToUserTimeString(dto.CreatedAt),
                    ["status"] = dto.IsActive
                        ? "<span class='badge badge-outline-danger'>Open</span>"
                        : "<span class='badge badge-outline-success'>Closed</span>"
                });
        }
        #endregion

        #region OpenTicket Details
        [HttpGet]
        public async Task<IActionResult> ViewOpenTicketDetails(string ticketUuid)
        {
           var vm = await BuildTicketDetailsVM(ticketUuid);

            if (vm == null)
                return RedirectToAction("ViewOpenTickets");

            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostReply(SupportTicketDetailsVM vm)
        {
            if (string.IsNullOrEmpty(vm.ReplyMessage) && (vm.Files == null || !vm.Files.Any()))
            {
                ModelState.AddModelError("", "Reply required");
            }

            if (!ModelState.IsValid)
            {
                var vmData = await BuildTicketDetailsVM(vm.TicketUUID);
                if (vmData != null)
                {
                    vm.Messages = vmData.Messages;
                }

                return View("ViewOpenTicketDetails", vm);
            }
            var attachmentPaths = new List<string>();
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf" };

            if (vm.Files != null && vm.Files.Any())
            {
                foreach (var file in vm.Files)
                {
                    var path = await _fileUploadService.SaveFileAsync(
                        file,
                        await GetCompanyNameAsync(),
                        "SupportTickets",
                        allowedExtensions);

                    attachmentPaths.Add(path);
                }
            }
            await _ticketHeaderService.AddReplyAsync(
                vm.TicketUUID,
                vm.ReplyMessage,
                attachmentPaths,
                GetUserUUID(),
                Utils.GetLocalIPAddress());

            if (string.IsNullOrWhiteSpace(vm.ReplyMessage) && !attachmentPaths.Any())
            {
                SetErrorMessage("Something went wrong");
            }
            else
            {
                SetSuccessMessage("Reply added successfully.");
            }

            return RedirectToAction(nameof(ViewOpenTicketDetails), new { ticketUuid = vm.TicketUUID });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CloseTicket(SupportTicketDetailsVM vm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(vm.TicketUUID))
                    return RedirectToAction("ViewSupportTickets");

                if (string.IsNullOrWhiteSpace(vm.Otp) || vm.Otp.Length != 6)
                {
                    SetErrorMessage("Invalid OTP.");
                    return RedirectToAction(nameof(ViewOpenTicketDetails), new { ticketUuid = vm.TicketUUID });
                }
                var success = await UpdateTicketStatusAsync(vm.TicketUUID, false, vm.Otp);

                if (!success)
                {
                    SetErrorMessage("Invalid OTP or ticket not found.");
                    return RedirectToAction(nameof(ViewOpenTicketDetails), new { ticketUuid = vm.TicketUUID });
                }

                SetSuccessMessage("Ticket closed successfully.");
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
            }

            return RedirectToAction("ViewOpenTickets");
        }
        #endregion


        #region Closed Tickets

        [HttpGet]

        public IActionResult ViewClosedTickets()
        {
            var vm = new SupportTicketsVM
            {
                UserTypeList = GetUserTypeList()
            };

            return View(vm);
        }

        [HttpPost]
        public Task<IActionResult> GetClosedTickets(string? userType, string? departmentUuid, string? assigneeUuid)
        {
            var currentUserUuid = GetUserUUID();
            return GetPagedDataAsync(
                request => _ticketHeaderService.GetPagedAsync(request, userType, departmentUuid, assigneeUuid, false, currentUserUuid),
                dto => new Dictionary<string, object>
                {
                    ["uuid"] = dto.UUID,
                    ["ticketnumber"] = dto.TicketNumber,
                    ["raisedby"] = BuildRaisedByHtml(dto),
                    ["subject"] = dto.Subject,
                    ["message"] = dto.Message,
                    ["createdat"] = ToUserTimeString(dto.CreatedAt),
                    ["status"] = dto.IsActive
                        ? "<span class='badge badge-outline-danger'>Open</span>"
                        : "<span class='badge badge-outline-success'>Closed</span>"
                });
        }
        #endregion

        #region ClosedTicket Details

        [HttpGet]
        public async Task<IActionResult> ViewClosedTicketDetails(string ticketUuid)
        {
            var vm = await BuildTicketDetailsVM(ticketUuid);

            if (vm == null)
                return RedirectToAction("ViewClosedTickets");

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReopenTicket(string ticketUuid)
        {
            try
            {
                var success = await UpdateTicketStatusAsync(ticketUuid, true);

                if (!success)
                {
                    SetErrorMessage("Ticket not found.");
                    return RedirectToAction("ViewClosedTickets");
                }

                SetSuccessMessage("Ticket reopened successfully.");
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
            }

            return RedirectToAction(nameof(ViewOpenTicketDetails), new { ticketUuid });
        }
        #endregion



    }
}


