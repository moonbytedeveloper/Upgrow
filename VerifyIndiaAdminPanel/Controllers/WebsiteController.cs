using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using Upgrow.Application;
using Upgrow.Application.Commands.Api;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.Commands.Website;
using Upgrow.Application.DTO;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.DTO.Website;
using Upgrow.Application.IServices;
using Upgrow.Application.IServices.Master;
using Upgrow.Application.IServices.Website;
using Upgrow.Application.Services.WL;
using Upgrow.Infrastructure.Filters;
using UpgrowAdminPanel.Models.Master;
using UpgrowAdminPanel.Models.Website;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace UpgrowAdminPanel.Controllers
{
    [ActivityLog]
    public class WebsiteController : BaseController
    {

        private readonly IClientsService _clientsService;
        private readonly IFileUploadService _fileUploadService;
        private readonly IWebHostEnvironment _environment;
        private readonly ICerificationsService _certificationsService;
        private readonly IKnowledgeHubCategoryService _knowledgeHubCategoryService;
        private readonly IKnowledgeHubService _knowledgeHubService;
        private readonly INewsService _newsService;
        private readonly INewsCategoryService _newscategoryService;
        private readonly IOurTeamService _ourTeamService;
        private readonly IMasterDesignationService _designationService;
        private readonly IWebsiteMasterFaqService _websitefaqService;
        private readonly IWebsiteFaqCategoryService _websitefaqCategoryService;
        private readonly IMasterSkillsService _websiteSkillsService;
        private readonly IMasterJobTypeService _websiteJobTypeService;
        private readonly IDomainResolverService _domainResolverService;
        private readonly IWebsite_VerificationServiceCategoryService _websiteVerificationServiceCategoryService;
        private readonly IWebsite_VerificationServiceService _websiteVerificationService;

        public WebsiteController(
            IWebsite_VerificationServiceCategoryService websiteVerificationServiceCategoryService,
            IWebsite_VerificationServiceService websiteVerificationService,
            IEncryptionService encryptionService,
            IClientsService clientsService,
            IDataTableParser dataTableParser,
            IFileUploadService fileUploadService,
            IWebHostEnvironment environment,
            ICerificationsService certificationsService,
            IKnowledgeHubCategoryService knowledgeHubCategoryService,
            IKnowledgeHubService knowledgeHubService,
            INewsService newsService,
            INewsCategoryService newscategoryService,
            IOurTeamService ourTeamService,
            IMasterDesignationService masterDesignationService,
            IWebsiteFaqCategoryService websitefaqCategoryService,
            IWebsiteMasterFaqService websiteMasterFaqService,
            IMasterSkillsService websiteSkillsService,
            IMasterJobTypeService websiteJobTypeService,
            IDomainResolverService domainResolverService
            ) : base(dataTableParser, domainResolverService, encryptionService)
        {
            _clientsService = clientsService;
            _fileUploadService = fileUploadService;
            _environment = environment;
            _certificationsService = certificationsService;
            _newsService = newsService;
            _newscategoryService = newscategoryService;
            _knowledgeHubCategoryService = knowledgeHubCategoryService;
            _knowledgeHubService = knowledgeHubService;
            _ourTeamService = ourTeamService;
            _designationService = masterDesignationService;
            _websitefaqService = websiteMasterFaqService;
            _websitefaqCategoryService = websitefaqCategoryService;
            _websiteSkillsService = websiteSkillsService;
            _websiteJobTypeService = websiteJobTypeService;
            _domainResolverService = domainResolverService;
            _websiteVerificationServiceCategoryService = websiteVerificationServiceCategoryService;
            _websiteVerificationService = websiteVerificationService;
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
        #region Clients

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Clients List", MenuName = "Clients")]
        public IActionResult ViewClients() => View();

        [HttpPost]
        public Task<IActionResult> GetClients()
        => GetPagedDataAsync<ClientsDto, ClientsCommand>(_clientsService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["name"] = dto.Name,
            ["icon"] = !string.IsNullOrEmpty(dto.IconImage)
                  ? $"<img src='{_domainResolverService.BuildAbsoluteUrl(dto.IconImage)}' alt='Client Icon' style='width: 80px; height: 50px; object-fit: cover; border-radius: 4px;' />"
                  : "",
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleClients), "client")
        });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Client Page", MenuName = "Clients")]
        public IActionResult AddClients() => View("AddClients", new ClientsCommand { IsActive = true });

        [HttpGet]
        public Task<IActionResult> EditClients(string? uuid)
            => EditMasterAsync(
                uuid,
                _clientsService,
                () => new ClientsCommand { IsActive = true },
                dto => new ClientsCommand
                {
                    UUID = dto.UUID,
                    Name = dto.Name,
                    IconImage = dto.IconImage,
                    ImageUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(dto.IconImage),
                    IsActive = dto.IsActive
                },
                "AddClients",
                nameof(ViewClients));

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Client Form", MenuName = "Clients")]
        public async Task<IActionResult> AddClients(ClientsCommand command)
        {
            if (!ModelState.IsValid)
            {

                return View("AddClients", command);
            }

            try
            {
                //var command = vm.Command;
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };


                if (command.Image != null)
                {

                    command.IconImage = await _fileUploadService.SaveFileAsync(
                        command.Image,
                            await GetCompanyNameAsync(),
                        "Clients",
                        allowedExtensions);
                    command.ImageUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(command.IconImage);
                }

                await _clientsService.SaveAsync(command, GetUserUUID(), Utils.GetLocalIPAddress());

                SetSuccessMessage(string.IsNullOrEmpty(command.UUID)
                    ? "Client added successfully!"
                    : "Client updated successfully!");

                return RedirectToAction(nameof(ViewClients));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);

                return View("AddClients", command);
            }
        }

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Client Status", MenuName = "Clients")] 
        public Task<IActionResult> ToggleClients(string uuid)
        => ToggleActiveAsync<ClientsDto, ClientsCommand>(uuid, _clientsService, "Client");

        #endregion

        #region Certifications

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Certifications List", MenuName = "Certifications")]
        public IActionResult ViewCertifications() => View();

        [HttpPost]
        public Task<IActionResult> GetCertifications()
        => GetPagedDataAsync<CertificationsDto, CertificationsCommand>(_certificationsService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["name"] = dto.Name,
            ["image"] = !string.IsNullOrEmpty(dto.FilePath)
                  ? $"<img src='{_domainResolverService.BuildAbsoluteUrl(dto.FilePath)}' alt='Certificate Image' style='width: 50px; height: 50px; object-fit: cover; border-radius: 4px;' />"
                  : "",
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleCertification), "certification")
        });

        [HttpGet]
            [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Certification Page", MenuName = "Certifications")]
        public IActionResult AddCertification() => View("AddCertification", new CertificationsCommand { IsActive = true });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Certification Page", MenuName = "Certifications")]
        public Task<IActionResult> EditCertification(string? uuid)
            => EditMasterAsync(
                uuid,
                _certificationsService,
                () => new CertificationsCommand { IsActive = true },
                dto => new CertificationsCommand
                {
                    UUID = dto.UUID,
                    Name = dto.Name,
                    FilePath = dto.FilePath,
                    ImageUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(dto.FilePath),
                    IsActive = dto.IsActive
                },
                "AddCertification",
                nameof(ViewCertifications));

        [HttpPost]
        [ValidateAntiForgeryToken]
            [ActivityLog(ActivityType = "Click", Description = "Clicked On Certification Form", MenuName = "Certifications")]
        public async Task<IActionResult> AddCertification(CertificationsCommand command)
        {
            if (!ModelState.IsValid)
            {

                return View("AddCertification", command);
            }

            try
            {
                //var command = vm.Command;
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".svg", ".mp4" };


                if (command.Image != null)
                {

                    command.FilePath = await _fileUploadService.SaveFileAsync(
                        command.Image,
                        await GetCompanyNameAsync(),
                        "Certifications",
                        allowedExtensions);
                    command.ImageUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(command.FilePath);
                }

                await _certificationsService.SaveAsync(command, GetUserUUID(), Utils.GetLocalIPAddress());

                SetSuccessMessage(string.IsNullOrEmpty(command.UUID)
                    ? "Certification added successfully!"
                    : "Certification updated successfully!");

                return RedirectToAction(nameof(ViewCertifications));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);

                return View("AddCertification", command);
            }
        }

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Certification Status", MenuName = "Certifications")]
        public Task<IActionResult> ToggleCertification(string uuid)
        => ToggleActiveAsync<CertificationsDto, CertificationsCommand>(uuid, _certificationsService, "Certification");

        #endregion

        #region News Category
        // Developed By : Krishna
        // Date : 30-03-2026
        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened News Category List", MenuName = "News_Category")]
        public IActionResult ViewNewsCategory() => View();

        [HttpPost]
        public Task<IActionResult> GetNewsCategory()
=> GetPagedDataAsync<NewsCategoryDto, NewsCategoryCommand>(_newscategoryService, dto => new Dictionary<string, object>
{
    ["uuid"] = dto.UUID,
    ["title"] = dto.Title,
   
    ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleNewsCategory), "NewsCategory")
});

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add News Category Page", MenuName = "News_Category")]
        public IActionResult AddNewsCategory() => View("AddNewsCategory", new NewsCategoryCommand { IsActive = true });

        [HttpGet]
            [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit News Category Page", MenuName = "News_Category")]
        public Task<IActionResult> EditNewsCategory(string? uuid)
            => EditMasterAsync(
                uuid,
                _newscategoryService,
                () => new NewsCategoryCommand { IsActive = true },
                dto => new NewsCategoryCommand
                {
                    UUID = dto.UUID,
                    Title = dto.Title,
                    ShortDescription = dto.ShortDescription,
                    IsActive = dto.IsActive
                },
                "AddNewsCategory",
                nameof(ViewNewsCategory));

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On News Category Form", MenuName = "News_Category")]
        public Task<IActionResult> AddNewsCategory(NewsCategoryCommand command)
            => SaveMasterAsync(
                command,
                _newscategoryService,
                "NewsCategory",
                "AddNewsCategory",
                nameof(ViewNewsCategory));

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled News Category Status", MenuName = "News_Category")]
        public Task<IActionResult> ToggleNewsCategory(string uuid)
            => ToggleActiveAsync<NewsCategoryDto, NewsCategoryCommand>(uuid, _newscategoryService, "NewsCategory");

        #endregion

        #region News
        // Developed By : Krishna
        // Date : 30-03-2026
        [HttpGet]
            [ActivityLog(ActivityType = "View", Description = "Opened News List", MenuName = "News")]
        public IActionResult ViewNews() => View();

        [HttpPost]
        public Task<IActionResult> GetNews()
        => GetPagedDataAsync<NewsDto, NewsCommand>(_newsService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["title"] = dto.Title,
            ["newcategory"] = dto.NewsCategoryUUID,
            ["publishdate"] = dto.PublishDate?.ToString("dd-MMM-yyyy") ?? "",           
            ["shortDescription"] = dto.ShortDescription,
            ["istopstory"] = dto.IsTopStory
                ? "<span class='badge badge-outline-success'>True</span>"
                : "<span class='badge badge-outline-danger'>False</span>",
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleNews), "News")
        });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add News Page", MenuName = "News")]
        public async Task<IActionResult> AddNews()
        {
            var newscategory = await _newscategoryService.GetDropdownAsync(x => x.Title);
            var selectItems = newscategory.Select(x => new SelectListItem
            {
                Value = x.UUID,
                Text = x.Title,
            }).ToList();

            var vm = new NewsVM
            {
                CategoryList = selectItems
            };

            vm.News = new NewsCommand { IsActive = true };
            return View("AddNews", vm);

        }

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit News Page", MenuName = "News")]
        public async Task<IActionResult> EditNews(string? uuid)
        {
            // Load email credentials for dropdown
            var newscategory = await _newscategoryService.GetDropdownAsync(x => x.Title);
            var selectItems = newscategory.Select(x => new SelectListItem
            {
                Value = x.UUID,
                Text = x.Title,
            }).ToList();

            var vm = new NewsVM
            {
                CategoryList = selectItems
            };

            if (string.IsNullOrEmpty(uuid))
            {
                vm.News = new NewsCommand { IsActive = true };
                return View("AddNews", vm);
            }

            var dto = await _newsService.GetByUuidAsync(uuid);

            if (dto == null)
            {
                SetErrorMessage("Record not found!");
                return RedirectToAction(nameof(ViewNews));
            }

            vm.News = new NewsCommand
            {
                UUID = dto.UUID,
                NewsCategoryUUID = dto.NewsCategoryUUID,
                Title = dto.Title,
                ShortDescription = dto.ShortDescription,
                FullDescription = dto.FullDescription,
                Location = dto.Location,
                Source = dto.Source,
                PublishDate = dto.PublishDate,
                CardImage = dto.CardImage,
                Image = dto.Image,
                ImageUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(dto.Image),
                CardImageUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(dto.CardImage),
                IsActive = dto.IsActive
            };

            return View("AddNews", vm);
        }

      
[HttpPost]
[ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On News Form", MenuName = "News")]
        public async Task<IActionResult> AddNews(NewsVM vm)
        {
            // ✅ FIX: Use BaseController's LoadDropdownAsync to populate CategoryList FIRST
            // This ensures dropdown stays populated even if validation fails
            vm.CategoryList = await LoadDropdownAsync<NewsCategoryDto, NewsCategoryCommand>(
                _newscategoryService,
                x => x.Title);
            try
            {
                if (!ModelState.IsValid)
            {
                 await LoadDropdowns(vm);
                    return View("AddNews", vm);
            }

           
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

                if (vm.News.ImageFile != null)
                {
                    vm.News.Image = await _fileUploadService.SaveFileAsync(
                        vm.News.ImageFile,
                         await GetCompanyNameAsync(),
                        "news",
                        allowedExtensions);
                    vm.News.ImageUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(vm.News.Image);
                }
                if (vm.News.CardImageFile != null)
                { 
                    vm.News.CardImage = await _fileUploadService.SaveFileAsync(
                       vm.News.CardImageFile,
                        await GetCompanyNameAsync(),
                       "news",
                       allowedExtensions);

                    vm.News.CardImageUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(vm.News.CardImage);

                }
                if (string.IsNullOrEmpty(vm.News.UUID) && string.IsNullOrEmpty(vm.News.Image) && string.IsNullOrEmpty(vm.News.CardImage))
                {
                    ModelState.AddModelError("News.Image", "News image is required.");
                    await LoadDropdowns(vm); ;
                    return View("AddNews", vm);
                }
                await _newsService.SaveAsync(vm.News, GetUserUUID(), Utils.GetLocalIPAddress());

                SetSuccessMessage(string.IsNullOrEmpty(vm.News.UUID)
                    ? "News added successfully!"
                    : "News updated successfully!");

                return RedirectToAction(nameof(ViewNews));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                return View("AddNews", vm);
            }
        }
        private async Task LoadDropdowns(NewsVM vm)
        {
            var news = await _newscategoryService.GetDropdownAsync(x => x.Title);
            vm.CategoryList = news.Select(x => new SelectListItem
            {
                Value = x.UUID,
                Text = x.Title
            }).ToList();
        }
            [HttpPost]
            [ActivityLog(ActivityType = "Update", Description = "Toggled News Status", MenuName = "News")]
        public Task<IActionResult> ToggleNews(string uuid)
            => ToggleActiveAsync<NewsDto, NewsCommand>(uuid, _newsService, "News");
        #endregion

        #region KnowledgeHubCategory

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Knowledge Hub Category List", MenuName = "Knowledge_Hub_Category")]
        public IActionResult ViewKnowledgeHubCategory() => View();

        [HttpPost]
        public Task<IActionResult> GetKnowledgeHubCategories()
        => GetPagedDataAsync<KnowledgeHubCategoryDto, KnowledgeHubCategoryCommand>(_knowledgeHubCategoryService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["name"] = dto.Name,
            ["descripttion"] = dto.ShortDescription,
            ["image"] = !string.IsNullOrEmpty(dto.IconImage)
                  ? $"<img src='{_domainResolverService.BuildAbsoluteUrl(dto.IconImage)}' alt='Category Image' style='width: 50px; height: 50px; object-fit: cover; border-radius: 4px;' />"
                  : "",
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleKnowledgeHubCategory), "knowledge Hub Category")
        });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Knowledge Hub Category Page", MenuName = "Knowledge_Hub_Category")]
        public IActionResult AddKnowledgeHubCategory() => View("AddKnowledgeHubCategory", new KnowledgeHubCategoryCommand { IsActive = true });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Knowledge Hub Category Page", MenuName = "Knowledge_Hub_Category")]
        public Task<IActionResult> EditKnowledgeHubCategory(string? uuid)
            => EditMasterAsync(
                uuid,
                _knowledgeHubCategoryService,
                () => new KnowledgeHubCategoryCommand { IsActive = true },
                dto => new KnowledgeHubCategoryCommand
                {
                    UUID = dto.UUID,
                    Name = dto.Name,
                    ShortDescription = dto.ShortDescription,
                    IconImage = dto.IconImage,
                    ImageUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(dto.IconImage),
                    IsActive = dto.IsActive
                },
                "AddKnowledgeHubCategory",
                nameof(ViewKnowledgeHubCategory));

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Knowledge Hub Category Form", MenuName = "Knowledge_Hub_Category")]
        public async Task<IActionResult> AddKnowledgeHubCategory(KnowledgeHubCategoryCommand command)
        {
            if (!ModelState.IsValid)
            {

                return View("AddKnowledgeHubCategory", command);
            }

            try
            {
                //var command = vm.Command;
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };


                if (command.Image != null)
                {

                    command.IconImage = await _fileUploadService.SaveFileAsync(
                        command.Image,
                        await GetCompanyNameAsync(),
                        "KnoweldegeHub",
                        allowedExtensions);
                    command.ImageUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(command.IconImage);
                }

                await _knowledgeHubCategoryService.SaveAsync(command, GetUserUUID(), Utils.GetLocalIPAddress());

                SetSuccessMessage(string.IsNullOrEmpty(command.UUID)
                    ? "Data added successfully!"
                    : "Data Category updated successfully!");

                return RedirectToAction(nameof(ViewKnowledgeHubCategory));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);

                return View("AddKnowledgeHubCategory", command);
            }
        }

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Knowledge Hub Category Status", MenuName = "Knowledge_Hub_Category")]
        public Task<IActionResult> ToggleKnowledgeHubCategory(string uuid)
        => ToggleActiveAsync<KnowledgeHubCategoryDto, KnowledgeHubCategoryCommand>(uuid, _knowledgeHubCategoryService, "Knowledge Hub Category");

        #endregion

        #region KnowledgeHub
        [HttpGet]
            [ActivityLog(ActivityType = "View", Description = "Opened Knowledge Hub List", MenuName = "Knowledge_Hub")]
        public IActionResult ViewKnowledgeHub() => View();

        [HttpPost]
        public Task<IActionResult> GetKnowledgeHub()
        => GetPagedDataAsync<KnowledgeHubDto, KnowledgeHubCommand>(_knowledgeHubService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["name"] = dto.Name,
            ["category"] = dto.CategoryUUID,
            ["description"] = dto.Description,
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleKnowledgeHub), "knowledge Hub")
        });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Knowledge Hub Page", MenuName = "Knowledge_Hub")]
        public async Task<IActionResult> AddKnowledgeHub()
        {
            var vm = new KnowledgeHubVM();

            // Load all dropdowns
            await LoadCategories(vm);

             // Add new employee
            vm.KnowledgeHub = new KnowledgeHubCommand { IsActive = true };
            return View("AddKnowledgeHub", vm);
            
        } 

        [HttpGet]
            [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Knowledge Hub Page", MenuName = "Knowledge_Hub")]
        public async Task<IActionResult> EditKnowledgeHub(string? uuid)
        {
            var vm = new KnowledgeHubVM();

            // Load all dropdowns
            await LoadCategories(vm);

            if (string.IsNullOrEmpty(uuid))
            {
                // Add new employee
                vm.KnowledgeHub = new KnowledgeHubCommand { IsActive = true };
                return View("AddKnowledgeHub", vm);
            }
            // Load existing employee
            var dto = await _knowledgeHubService.GetByUuidAsync(uuid);
            if (dto == null)
            {
                SetErrorMessage("Record not found!");
                return RedirectToAction(nameof(ViewKnowledgeHub));
            }

            vm.KnowledgeHub = new KnowledgeHubCommand
            {
                UUID = dto.UUID,
                Name = dto.Name,
                ImageURL= dto.ImageURL,
                ImageUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(dto.ImageURL),
                Description = dto.Description,
                CategoryUUID = dto.CategoryUUID,
                LongDescription = dto.LongDescription,
                SequenceNo = dto.SequenceNo,
                IsActive = dto.IsActive
            };

            return View("AddKnowledgeHub", vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Knowledge Hub Form", MenuName = "Knowledge_Hub")]
        public async Task<IActionResult> AddKnowledgeHub(KnowledgeHubVM vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await LoadCategories(vm);
                    return View("AddKnowledgeHub", vm);
                }

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };


                if (vm.KnowledgeHub.Image != null)
                {

                    vm.KnowledgeHub.ImageURL = await _fileUploadService.SaveFileAsync(
                        vm.KnowledgeHub.Image,
                        await GetCompanyNameAsync(),
                        "KnoweldegeHub",
                        allowedExtensions);
                    vm.KnowledgeHub.ImageUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(vm.KnowledgeHub.ImageURL);
                }
                await _knowledgeHubService.SaveAsync(
                    vm.KnowledgeHub,
                    GetUserUUID(),
                    Utils.GetLocalIPAddress());

                SetSuccessMessage(string.IsNullOrEmpty(vm.KnowledgeHub.UUID)
                    ? "Data added successfully!"
                    : "Data updated successfully!");

                return RedirectToAction(nameof(ViewKnowledgeHub));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                await LoadCategories(vm);
                return View("AddKnowledgeHub", vm);
            }
        }
        private async Task LoadCategories(KnowledgeHubVM vm)
        {
            var category = await _knowledgeHubCategoryService.GetDropdownAsync(x => x.Name);
            vm.Categories = category.Select(x => new SelectListItem
            {
                Value = x.UUID,
                Text = x.Title
            }).ToList();

        }
        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Knowledge Hub Status", MenuName = "Knowledge_Hub")]
        public Task<IActionResult> ToggleKnowledgeHub(string uuid)
           => ToggleActiveAsync<KnowledgeHubDto, KnowledgeHubCommand>(uuid, _knowledgeHubService, "Knowledge Hub");
        #endregion

        #region OurTeam
        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Our Team List", MenuName = "OurTeam")]
        public IActionResult ViewOurTeam() => View();

        [HttpPost]
        public Task<IActionResult> GetOurTeams()
        => GetPagedDataAsync<OurTeamDto, OurTeamCommand>(_ourTeamService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["name"] = dto.Name,
            ["designation"] = dto.Designation,
            ["description"] = dto.Description,
            ["image"] = !string.IsNullOrEmpty(dto.ImageFile)
                  ? $"<img src='{_domainResolverService.BuildAbsoluteUrl(dto.ImageFile)}' alt='Team Image' style='width: 50px; height: 50px; object-fit: cover; border-radius: 4px;' />"
                  : "",
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleOurTeam), "ourTeam")
        });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Our Team Page", MenuName = "OurTeam")]
        public IActionResult AddOurTeam() => View("AddOurTeam", new OurTeamCommand { IsActive = true });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Our Team Page", MenuName = "OurTeam")]
        public Task<IActionResult> EditOurTeam(string? uuid)
            => EditMasterAsync(
                uuid,
                _ourTeamService,
                () => new OurTeamCommand { IsActive = true },
                dto => new OurTeamCommand
                {
                    UUID = dto.UUID,
                    Name = dto.Name,
                    ImageFile = dto.ImageFile,
                    ImageUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(dto.ImageFile),
                    Designation = dto.Designation,
                    IconURL = dto.IconURL,
                    Description = dto.Description,
                    IsActive = dto.IsActive
                },
                "AddOurTeam",
                nameof(ViewOurTeam));

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Our Team Form", MenuName = "OurTeam")]
        public async Task<IActionResult> AddOurTeam(OurTeamCommand command)
        {
            if (!ModelState.IsValid)
            {
                return View("AddOurTeam", command);
            }
            try
            {
                //var command = vm.Command;
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };


                if (command.Image != null)
                {

                    command.ImageFile = await _fileUploadService.SaveFileAsync(
                        command.Image,
                        "OurTeam",
                        await GetCompanyNameAsync(),
                        allowedExtensions);
                    command.ImageUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(command.ImageFile);
                }

                await _ourTeamService.SaveAsync(command, GetUserUUID(), Utils.GetLocalIPAddress());

                SetSuccessMessage(string.IsNullOrEmpty(command.UUID)
                    ? "Data added successfully!"
                    : "Data updated successfully!");

                return RedirectToAction(nameof(ViewOurTeam));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);

                return View("AddOurTeam", command);
            }
        }

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Our Team Status", MenuName = "OurTeam")]
        public Task<IActionResult> ToggleOurTeam(string uuid)
        => ToggleActiveAsync<OurTeamDto, OurTeamCommand>(uuid, _ourTeamService, "OurTeam");

        #endregion

        #region FAQ Category
        // Developed by Krishna (17-06-2026)
        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Website FAQ Category List", MenuName = "Website_FAQ_Category")]
        public IActionResult ViewFaqCategory() => View();

        [HttpPost]
        public Task<IActionResult> GetFaqCategory()
        => GetPagedDataAsync<WebsiteFaqCategoryDto, WebsiteFaqCategoryCommand>(_websitefaqCategoryService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["name"] = dto.Title,
            ["icon"] = dto.Icon,
            ["shortdescription"] = dto.ShortDescription,
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleFaqCategory), "FAQ Category")
        });

       

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Website FAQ Category", MenuName = "Website_FAQ_Category")]
        public IActionResult AddFaqCategory() => View("AddFaqCategory", new WebsiteFaqCategoryCommand { IsActive = true });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Website FAQ Category", MenuName = "Website_FAQ_Category")]
        public Task<IActionResult> EditFaqCategory(string? uuid)
            => EditMasterAsync(
                uuid,
                _websitefaqCategoryService,
                () => new WebsiteFaqCategoryCommand { IsActive = true },
                dto => new WebsiteFaqCategoryCommand
                {
                    UUID = dto.UUID,
                    Title = dto.Title,
                    Icon = dto.Icon,
                    ShortDescription = dto.ShortDescription,
                    IsActive = dto.IsActive
                },
                "AddFaqCategory",
                nameof(ViewFaqCategory));

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Website FAQ Category", MenuName = "Website_FAQ_Category")]
        public Task<IActionResult> AddFaqCategory(WebsiteFaqCategoryCommand command)
            => SaveMasterAsync(
                command,
                _websitefaqCategoryService,
                "FaqCategory",
                "AddFaqCategory",
                nameof(ViewFaqCategory));

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Website FAQ Category Status", MenuName = "Website_FAQ_Category")]
        public Task<IActionResult> ToggleFaqCategory(string uuid)
            => ToggleActiveAsync<WebsiteFaqCategoryDto, WebsiteFaqCategoryCommand>(uuid, _websitefaqCategoryService, "FAQ Category");

        #endregion

        #region FAQ
        // Developed by Krishna (17-06-2026)

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Master FAQ List", MenuName = "Website_MasterFAQ")]
        public IActionResult MasterViewFaq() => View();

        [HttpPost]
        public Task<IActionResult> GetFaq()
          => GetPagedDataAsync<WebsiteMasterFaqDto, WebsiteMasterFaqCommand>(_websitefaqService, dto => new Dictionary<string, object>
          {
              ["uuid"] = dto.UUID,
              ["title"] = dto.Title ?? "",
              ["category"] = dto.FAQCategoryUUID ?? "",
              ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleFaq), "FAQ")
          });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master FAQ", MenuName = "Master_FAQ")]
        public async Task<IActionResult> MasterAddFaq()
        {
            var vm = new FaqVM();
            vm.FaqCategoryList = await LoadDropdownAsync(_websitefaqCategoryService, x => x.Title);
           
            // Populate dropdowns first so the view always has required lists
            vm.Faq = new WebsiteMasterFaqCommand { IsActive = true };
            return View("MasterAddFaq", vm);

        }

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Master FAQ", MenuName = "Master_FAQ")]
        public async Task<IActionResult> MasterEditFaq(string? uuid)
        {
            var vm = new FaqVM();
            vm.FaqCategoryList = await LoadDropdownAsync(_websitefaqCategoryService, x => x.Title);
            
            // Populate dropdowns first so the view always has required lists

            if (string.IsNullOrEmpty(uuid))
            {
                vm.Faq = new WebsiteMasterFaqCommand { IsActive = true };
                return View("MasterAddFaq", vm);
            }

            var dto = await _websitefaqService.GetByUuidAsync(uuid);

            if (dto == null)
            {
                SetErrorMessage("Record not found!");
                return RedirectToAction(nameof(MasterViewFaq));
            }

            vm.Faq = new WebsiteMasterFaqCommand
            {
                UUID = dto.UUID,
                FAQCategoryUUID = dto.FAQCategoryUUID,
                Title = dto.Title,
                Description = dto.Description,
                IsActive = dto.IsActive,
                IsFeatured = dto.IsFeatured
            };

            return View("MasterAddFaq", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master FAQ", MenuName = "Master_FAQ")]
        public async Task<IActionResult> MasterAddFaq(FaqVM vm)
        {
            if (vm == null)
                return BadRequest();

            // Validate inner command model
            if (!TryValidateModel(vm.Faq, nameof(vm.Faq)))
            {
                vm.FaqCategoryList = await LoadDropdownAsync(_websitefaqCategoryService, x => x.Title);
                // repopulate all dropdowns on validation failure
                return View("MasterAddFaq", vm);
            }

            try
            {
                await _websitefaqService.SaveAsync(vm.Faq, GetUserUUID(), Utils.GetLocalIPAddress());

                SetSuccessMessage(string.IsNullOrEmpty(vm.Faq.UUID)
                    ? "FAQ added successfully!"
                    : "FAQ updated successfully!");

                return RedirectToAction(nameof(MasterViewFaq));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                vm.FaqCategoryList = await LoadDropdownAsync(_websitefaqCategoryService, x => x.Title);
                // Reload dropdowns on error
                return View("MasterAddFaq", vm);
            }
        }

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Master FAQ Status", MenuName = "Master_FAQ")]
        public Task<IActionResult> ToggleFaq(string uuid)
            => ToggleActiveAsync<WebsiteMasterFaqDto, WebsiteMasterFaqCommand>(uuid, _websitefaqService, "Faq");
        #endregion

        #region Skills      
        // Developed by Krishna (18-06-2026)

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Master Skills List", MenuName = "Master_Skills")]
        public IActionResult MasterViewSkills() => View();

        [HttpPost]

        public Task<IActionResult> GetSkills()
        => GetPagedDataAsync<MasterSkillsDto, MasterSkillsCommand>(_websiteSkillsService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["name"] = dto.Title,
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleSkills), "Skills")
        });
        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Skills", MenuName = "Master_Skills")]
        public IActionResult MasterAddSkills()
        {
            return View("MasterAddSkills", new MasterSkillsCommand
            {
                IsActive = true
            });
        }
        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Master Skills", MenuName = "Master_Skills")]
        public Task<IActionResult> MasterEditSkills(string? uuid)
            => EditMasterAsync(
                uuid,
                _websiteSkillsService,
                () => new MasterSkillsCommand { IsActive = true },
                dto => new MasterSkillsCommand
                {
                    UUID = dto.UUID,
                    Title = dto.Title,
                    IsActive = dto.IsActive
                },
                "MasterAddSkills",
                nameof(MasterViewSkills));

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Skills", MenuName = "Master_Skills")]
        public Task<IActionResult> MasterAddSkills(MasterSkillsCommand command)
            => SaveMasterAsync(
                command,
                _websiteSkillsService,
                "Skills",
                "MasterAddSkills",
                nameof(MasterViewSkills));

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Master Skills Status", MenuName = "Master_Skills")]
        public Task<IActionResult> ToggleSkills(string uuid)
            => ToggleActiveAsync<MasterSkillsDto, MasterSkillsCommand>(uuid, _websiteSkillsService, "Skills");
        #endregion

        #region Job Type      
        // Developed by Krishna (18-06-2026)

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Master JobType List", MenuName = "Master_JobType")]
        public IActionResult MasterViewJobType() => View();

        [HttpPost]

        public Task<IActionResult> GetJobTypes()
        => GetPagedDataAsync<MasterJobTypeDto, MasterJobTypeCommand>(_websiteJobTypeService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["name"] = dto.Title,
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleJobType), "Job Type")
        });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master JobType", MenuName = "Master_JobType")]
        public IActionResult MasterAddJobType()
        {
            return View("MasterAddJobType", new MasterJobTypeCommand
            {
                IsActive = true
            });
        }

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Master JobType", MenuName = "Master_JobType")]
        public Task<IActionResult> MasterEditJobType(string? uuid)
            => EditMasterAsync(
                uuid,
                _websiteJobTypeService,
                () => new MasterJobTypeCommand { IsActive = true },
                dto => new MasterJobTypeCommand
                {
                    UUID = dto.UUID,
                    Title = dto.Title,
                    IsActive = dto.IsActive
                },
                "MasterAddJobType",
                nameof(MasterViewJobType));

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Job Type", MenuName = "Master_JobType")]
        public Task<IActionResult> MasterAddJobType(MasterJobTypeCommand command)
            => SaveMasterAsync(
                command,
                _websiteJobTypeService,
                "JobType",
                "MasterAddJobType",
                nameof(MasterViewJobType));

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Master Job Type Status", MenuName = "Master_JobType")]
        public Task<IActionResult> ToggleJobType(string uuid)
            => ToggleActiveAsync<MasterJobTypeDto, MasterJobTypeCommand>(uuid, _websiteJobTypeService, "Job Type");
        #endregion

        #region WebsiteVerificationServiceCategory
        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Service Category List", MenuName = "Website_VerificationServiceCategory")]
        public IActionResult ViewServiceCategory() => View();

        [HttpPost]

        public Task<IActionResult> GetServiceCategories()
        => GetPagedDataAsync<Website_VerificationServiceCategoryDto, Website_VerificationServiceCategoryCommand>(_websiteVerificationServiceCategoryService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["title"] = dto.Title,
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleServiceCategory), "Service Category")
        });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Service Category", MenuName = "Website_VerificationServiceCategory")]
        public IActionResult AddServiceCategory()
        {
            return View("AddServiceCategory", new Website_VerificationServiceCategoryCommand
            {
                IsActive = true
            });
        }

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Service Category", MenuName = "Website_VerificationServiceCategory")]
        public Task<IActionResult> EditServiceCategory(string? uuid)
            => EditMasterAsync(
                uuid,
                _websiteVerificationServiceCategoryService,
                () => new Website_VerificationServiceCategoryCommand { IsActive = true },
                dto => new Website_VerificationServiceCategoryCommand
                {
                    UUID = dto.UUID,
                    Title = dto.Title,
                    IsActive = dto.IsActive
                },
                "AddServiceCategory",
                nameof(ViewServiceCategory));

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Service Category", MenuName = "Website_VerificationServiceCategory")]
        public Task<IActionResult> AddServiceCategory(Website_VerificationServiceCategoryCommand command)
            => SaveMasterAsync(
                command,
                _websiteVerificationServiceCategoryService,
                "WebsiteVerificationServiceCategory",
                "AddServiceCategory",
                nameof(ViewServiceCategory));        
                
        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Service Category Status", MenuName = "Website_VerificationServiceCategory")]
        public Task<IActionResult> ToggleServiceCategory(string uuid)
            => ToggleActiveAsync<Website_VerificationServiceCategoryDto, Website_VerificationServiceCategoryCommand>(uuid, _websiteVerificationServiceCategoryService, "Website Verification Service Category");

        #endregion

        #region WebsiteVerificationService
        private async Task LoadCategoryDropdown(ServiceVM vm)
        {
            var category = await _websiteVerificationServiceCategoryService.GetDropdownAsync(x => x.Title);
            vm.ServiceCategoryList = category.Select(x => new SelectListItem
            {
                Value = x.UUID,
                Text = x.Title
            }).ToList();

        }

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Service List", MenuName = "Website_VerificationService")]
        public IActionResult  ViewService() => View();

        [HttpPost]
        public Task<IActionResult> GetServices()
        => GetPagedDataAsync<Website_VerificationServiceDto, Website_VerificationServiceCommand>(_websiteVerificationService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["category"] = dto.ServiceCategoryUUID,
            ["title"] = dto.Title,
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleService), "service")
        });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Service", MenuName = "Website_VerificationService")]
        public async Task<IActionResult> AddService()
        {
            var vm = new ServiceVM();
            await LoadCategoryDropdown(vm);

            vm.Service = new Website_VerificationServiceCommand { IsActive = true };
            return View("AddService", vm);
        }

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Service", MenuName = "Website_VerificationService")]
        public async Task<IActionResult> EditService(string? uuid)
        {
            var vm = new ServiceVM();
            await LoadCategoryDropdown(vm);

            if (string.IsNullOrEmpty(uuid))
            {
                vm.Service = new Website_VerificationServiceCommand { IsActive = true };
                return View("AddService", vm);
            }

            var dto = await _websiteVerificationService.GetByUuidAsync(uuid);

            if (dto == null)
            {
                SetErrorMessage("Record not found!");
                return RedirectToAction(nameof(ViewService));
            }

            vm.Service = new Website_VerificationServiceCommand
            {
                UUID = dto.UUID,
                Title = dto.Title,
                ServiceCategoryUUID = dto.ServiceCategoryUUID,
                IsActive = dto.IsActive
            };

            return View("AddService", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Service", MenuName = "Website_VerificationService")]
        public async Task<IActionResult> AddService(ServiceVM vm)
        {
            if (vm == null)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                await LoadCategoryDropdown(vm);
                return View("AddService", vm);
            }

            try
            {
                await _websiteVerificationService.SaveAsync(vm.Service, GetUserUUID(), Utils.GetLocalIPAddress());

                SetSuccessMessage(string.IsNullOrEmpty(vm.Service.UUID)
                    ? "Service added successfully!"
                    : "Service updated successfully!");
                return RedirectToAction(nameof(ViewService));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);

                // Reload dropdown on error
                await LoadCategoryDropdown(vm);
                return View("AddService", vm);
            }
        }

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Service Status", MenuName = "Website_VerificationService")]
        public Task<IActionResult> ToggleService(string uuid)
            => ToggleActiveAsync<Website_VerificationServiceDto, Website_VerificationServiceCommand>(uuid, _websiteVerificationService, "Service");
        #endregion


    }
}
