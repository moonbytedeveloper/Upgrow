using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VerifyIndia.Application;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.Common;
using VerifyIndia.Application.DTO.Auth;
using VerifyIndia.Application.DTO.Inquiry;
using VerifyIndia.Application.Interfaces;
using VerifyIndia.Application.Interfaces.Notification;
using VerifyIndia.Application.Interfaces.Pricing;
using VerifyIndia.Application.IServices;
using VerifyIndia.Application.IServices.ActionLogs;
using VerifyIndia.Application.IServices.AIX;
using VerifyIndia.Application.IServices.Api;
using VerifyIndia.Application.IServices.Auth;
using VerifyIndia.Application.IServices.Common;
using VerifyIndia.Application.IServices.Credential;
using VerifyIndia.Application.IServices.Inquiry;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Application.IServices.Menu;
using VerifyIndia.Application.IServices.Pricing;
using VerifyIndia.Application.IServices.Support;
using VerifyIndia.Application.IServices.Website;
using VerifyIndia.Application.IServices.WL;
using VerifyIndia.Application.IServices.WL.ActionLogs;
using VerifyIndia.Application.IServices.WL.Master;
using VerifyIndia.Application.Options;
using VerifyIndia.Application.Services;
using VerifyIndia.Application.Services.ActionLogs;
using VerifyIndia.Application.Services.AIX;
using VerifyIndia.Application.Services.Api;
using VerifyIndia.Application.Services.Auth;
using VerifyIndia.Application.Services.Credential;
using VerifyIndia.Application.Services.Inquiry;
using VerifyIndia.Application.Services.Master;
using VerifyIndia.Application.Services.Menu;
using VerifyIndia.Application.Services.Notification;
using VerifyIndia.Application.Services.Pricing;
using VerifyIndia.Application.Services.Support;
using VerifyIndia.Application.Services.Website;
using VerifyIndia.Application.Services.WL;
using VerifyIndia.Application.Services.WL.Master;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.Entities.Auth;
using VerifyIndia.Domain.Entities.Registration;
using VerifyIndia.Domain.Entities.Website;
using VerifyIndia.Domain.Entities.WL.Master;
using VerifyIndia.Domain.IRepositories;
using VerifyIndia.Domain.IRepositories.ActionLogs;
using VerifyIndia.Domain.IRepositories.Inquiry;
using VerifyIndia.Domain.IRepositories.LoginLogs;
using VerifyIndia.Domain.IRepositories.Master;
using VerifyIndia.Infrastructure;
using VerifyIndia.Infrastructure.Common;
using VerifyIndia.Infrastructure.Filters;
using VerifyIndia.Infrastructure.MultiTenancy;
using VerifyIndia.Infrastructure.Notifications;
using VerifyIndia.Infrastructure.Notifications.Channels;
using VerifyIndia.Infrastructure.Repositories;
using VerifyIndia.Infrastructure.Repositories.ActionLogs;
using VerifyIndia.Infrastructure.Repositories.Api;
using VerifyIndia.Infrastructure.Repositories.Inquiry;
using VerifyIndia.Infrastructure.Repositories.LoginLogs;
using VerifyIndia.Infrastructure.Repositories.Master;
using VerifyIndia.Infrastructure.Repositories.Menu;
using VerifyIndia.Infrastructure.Repositories.Pricing;
using VerifyIndia.Infrastructure.Repositories.Support;
using VerifyIndia.Infrastructure.Repositories.Website;
using VerifyIndia.Infrastructure.Repositories.WL.Master;
using VerifyIndia.Infrastructure.Services;
using UpgrowAdminPanel.Filters;

var builder = WebApplication.CreateBuilder(args);

#region Tenant Setup
builder.Services.Configure<AppInfo>(builder.Configuration.GetSection("App"));
builder.Services.AddMultiTenant<AppTenantInfo>()
                .WithHeaderStrategy("X-Tenant")
                .WithStore<TenantStore>(ServiceLifetime.Scoped);

builder.Services.AddScoped<ITenantSetupService, TenantSetupService>();
#endregion


builder.Services.AddAutoMapper(cfg => { }, typeof(AutoMapperProfile));

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.AddService<PathPermissionFilter>();
    options.Filters.AddService<ActivityLogAttribute>();  // ✅ ADD THIS LINE
})
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters
            .Add(new DateOnlyJsonConverter());
    });
// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();
// Add this section after AddHttpContextAccessor (around line 52)
builder.Services.AddScoped<ActivityLogAttribute>();  // ✅ Register as scoped service
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied";

        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromDays(2);
    });

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options.UseSqlServer(connectionString, sqlServerOptions =>
    {
        //sqlServerOptions.UseNetTopologySuite();
    }));


//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(connectionString, sqlServerOptions =>
//    {
//        //sqlServerOptions.UseNetTopologySuite();
//    }));

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString);

    options.EnableSensitiveDataLogging();

    options.LogTo(
        Console.WriteLine,
        LogLevel.Information);
});

builder.Services.Configure<FileUploadOptions>(builder.Configuration.GetSection("FileApi"));

builder.Services.AddHttpClient("FileApiUpload", (sp, client) =>
{
    var fileApiOptions = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<FileUploadOptions>>().Value;

    if (string.IsNullOrWhiteSpace(fileApiOptions.BaseUrl))
        throw new InvalidOperationException("FileApi:BaseUrl is missing in configuration.");

    client.BaseAddress = new Uri(fileApiOptions.BaseUrl, UriKind.Absolute);
    client.Timeout = TimeSpan.FromSeconds(60);
});

builder.Services.Configure<EncryptionDto>(builder.Configuration.GetSection("EncryptionSettings"));

builder.Services.AddScoped<IEncryptionService, EncryptionService>();


#region DI : Services
builder.Services.AddScoped<IManageApiService, ManageApiService>();
builder.Services.AddScoped<ICommonService, CommonService>();
builder.Services.AddScoped<IMasterPricingService, MasterPricingService>();
builder.Services.AddScoped<IMasterCustomerService, MasterCustomerService>();
builder.Services.AddScoped<IMasterBusinessIndustryService, MasterBusinessIndustryService>();
builder.Services.AddScoped<IWebsiteIndustryService, WebsiteIndustryService>();
builder.Services.AddScoped<IAppSettingsService, AppSettingsService>();
builder.Services.AddScoped<IWLLoginAttemptService, WLLoginAttemptService>();
builder.Services.AddScoped<IPasswordHasher<MasterEmployeeCommand>, PasswordHasher<MasterEmployeeCommand>>();
builder.Services.AddScoped<IDataTableParser, DataTableParser>();
builder.Services.AddScoped<IWLLoginAttemptLogsService, WLLoginAttemptLogsService>();
builder.Services.AddScoped<IActionLogsService, ActionLogsService>();
builder.Services.AddScoped<IWLActionLogsService, WLActionLogsService>();
builder.Services.AddScoped<IApiXCategoryService, ApiXCategoryService>();
builder.Services.AddScoped<IApiXStatusCodesService, ApiXStatusCodesService>();
builder.Services.AddScoped<IWebsiteIndustryPointsService, WebsiteIndustryPointsService>();
builder.Services.AddScoped<IWebsiteIndustryCasesService, WebsiteIndustryCasesService>();

builder.Services.AddScoped<IWebsite_VerificationServiceCategoryService, Website_VerificationServiceCategoryService>();
builder.Services.AddScoped<IWebsite_VerificationServiceService, Website_VerificationServiceService>();

builder.Services.AddScoped<IApiInfoFieldsService, ApiInfoFieldsService>();
builder.Services.AddScoped<IApiInfoSectionService, ApiInfoSectionService>();


builder.Services.AddScoped<IVerificationRequestLogService, VerificationRequestLogService>();
builder.Services.AddScoped<IMasterGenderService, MasterGenderService>();
builder.Services.AddScoped<IMasterHonorificService, MasterHonorificService>();
builder.Services.AddScoped<IMasterApiCategoryService, MasterApiCategoryService>();
builder.Services.AddScoped<IMasterDesignationService, MasterDesignationService>();
builder.Services.AddScoped<IMasterPermissionService, MasterPermissionService>();
builder.Services.AddScoped<IMasterPathService, MasterPathService>();
builder.Services.AddScoped<IMasterPathPermissionService, MasterPathPermissionService>();
builder.Services.AddScoped<ILoginLogoutService, LoginLogoutService>();
builder.Services.AddScoped<IMasterApiService, MasterApiService>();
builder.Services.AddScoped<IMasterIndustryService, MasterIndustryService>();
builder.Services.AddScoped<IMasterDepartmentService, MasterDepartmentService>();
builder.Services.AddScoped<IMasterCountryService, MasterCountryService>();
builder.Services.AddScoped<IMasterStateService, MasterStateService>();
builder.Services.AddScoped<IMasterCityService, MasterCityService>();
builder.Services.AddScoped<IDropdownQueryService, DropdownQueryService>();
builder.Services.AddScoped<IMasterRoleService, MasterRoleService>();
builder.Services.AddScoped<IMasterMenuService, MasterMenuService>();
builder.Services.AddScoped<IAdminAuthLogsService, AdminAuthLogsService>();
builder.Services.AddScoped<ILoginAttemptLogsService, LoginAttemptLogsService>();
builder.Services.AddScoped<IWLMasterMenuService, WLMasterMenuService>();
builder.Services.AddScoped<IMasterEmailCredentialService, MasterEmailCredentialService>();
builder.Services.AddScoped<IMasterEmailTemplateService, MasterEmailTemplateService>();
builder.Services.AddScoped<IMasterEmployeeService, MasterEmployeeService>();
builder.Services.AddScoped<IMasterNomenclatureService, MasterNomenclatureService>();
builder.Services.AddScoped<IAdminAuthService, AdminAuthService>();
builder.Services.AddScoped<IActivityLogsLogsService, ActivityLogsLogsService>();
builder.Services.AddScoped<IWLMasterPermissionService, WLMasterPermissionService>();
builder.Services.AddScoped<IMenuRolePermissionService, MenuRolePermissionService>();
builder.Services.AddScoped<IMenuQueryService, MenuQueryService>();
builder.Services.AddScoped<IApiProviderService, ApiProviderService>();
builder.Services.AddScoped<IMasterPermissionGroupService, MasterPermissionGroupService>();
builder.Services.AddScoped<IApiComponentsService, ApiComponentsService>();
builder.Services.AddScoped<IApiProviderComponentMappingService, ApiProviderComponentMappingService>();
builder.Services.AddScoped<INotificationChannel, NotificationService>();
builder.Services.AddScoped<IApiProviderMappingService, ApiProviderMappingService>();
builder.Services.AddScoped<IWLMasterPathPermissionService, WLMasterPathPermissionService>();
builder.Services.AddScoped<IWLMasterPathService, WLMasterPathService>();
builder.Services.AddScoped<IProviderApisService, ProviderApisService>();
builder.Services.AddScoped<IMasterTestimonialService, MasterTestimonialService>();
builder.Services.AddScoped<IClientsService, ClientsService>();
builder.Services.AddScoped<IMasterCareerService, MasterCareerService>();
builder.Services.AddScoped<ICerificationsService, CertificationsService>();
builder.Services.AddScoped<IMasterFaqCategoryService, MasterFaqCategoryService>();
builder.Services.AddScoped<IMasterFaqService, MasterFaqService>();
builder.Services.AddScoped<IServiceCategoryService, ServiceCategoryService>();
builder.Services.AddScoped<IOurTeamService, OurTeamService>();
builder.Services.AddScoped<IMasterDosDontsService, MasterDosDontsService>();
builder.Services.AddScoped<ITrustedPartnersService, TrustedPartnersService>();
builder.Services.AddScoped<IKnowledgeHubCategoryService, KnowledgeHubCategoryService>();
builder.Services.AddScoped<IKnowledgeHubService, KnowledgeHubService>();
builder.Services.AddScoped<INewsCategoryService, NewsCategoryService>();
builder.Services.AddScoped<INewsService, NewsService>();
builder.Services.AddScoped<IMasterDocumentService, MasterDocumentService>();
//builder.Services.AddScoped<IMasterDepartmentService, MasterDepartmentService>();
//builder.Services.AddScoped<IMasterDesignationService, MasterDesignationService>();
//builder.Services.AddScoped<IMasterManagePathService, MasterManagePathService>();
//builder.Services.AddScoped<IMasterMenuService, MasterMenuService>();
builder.Services.AddScoped<IMasterSocialMediaService, MasterSocialMediaService>();
//builder.Services.AddScoped<IMasterBlogService, MasterBlogService>();
//builder.Services.AddScoped<IMasterBlogCategoryService, MasterBlogCategoryService>();
//builder.Services.AddScoped<IMasterCMSService, MasterCMSService>();
//builder.Services.AddScoped<IMasterSocialMediaURLService, MasterSocialMediaURLService>();
builder.Services.AddScoped<IMasterBlogService, MasterBlogService>();
builder.Services.AddScoped<IMasterBlogCategoryService, MasterBlogCategoryService>();
builder.Services.AddScoped<IMasterCMSService, MasterCMSService>();
//builder.Services.AddScoped<IFileUploadService, FileUploadService>();
//builder.Services.AddScoped<IMasterClientsService, MasterClientsService>();
//builder.Services.AddScoped<IMenuRoleService, MasterRoleService>();
builder.Services.AddScoped<IMasterBannerService, MasterBannerService>();
//builder.Services.AddScoped<IMasterBlogService, MasterBlogService>();
//builder.Services.AddScoped<IMasterBlogCategoryService, MasterBlogCategoryService>();
builder.Services.AddScoped<IFileUploadService, FileUploadService>();
builder.Services.AddScoped<IFileMalwareScanner, NoOpFileMalwareScanner>();
builder.Services.AddScoped<IMasterCompanyBasicDataService, MasterCompanyBasicDataService>();
//builder.Services.AddScoped<IMasterEmployeeService, MasterEmployeeService>();
//builder.Services.AddScoped<IMasterPathService, MasterPathService>();
//builder.Services.AddScoped<IProductService, ProductService>();

//builder.Services.AddScoped<IProductFeatures, ProductFeatureService>();
//builder.Services.AddScoped<IPagedService<InquiryProductDto>, InquiryProductService>();
//builder.Services.AddScoped<IPagedService<InquiryContactDto>, InquiryContactService>();
builder.Services.AddScoped<UpgrowAdminPanel.Filters.PathPermissionFilter>();
builder.Services.AddScoped<IMasterFinancialYearService, MasterFinancialYearService>();

builder.Services.AddScoped<IWLSocialMediaService, WLSocialMediaService>();
builder.Services.AddScoped<IWLMasterBannerService, WLMasterBannerService>();
builder.Services.AddScoped<IWLMasterCompanyBasicDataService, WLMasterComapnyBasicDataService>();
builder.Services.AddScoped<IWLClientsService, WLClientsService>();
// Ensure the concrete repository with the join is used

builder.Services.AddScoped<IActivityLogsService, ActivityLogsService>();
builder.Services.AddScoped<IWLTenantService, WLTenantService>();

builder.Services.AddScoped<IDomainResolverService, DomainResolverService>();
builder.Services.AddScoped<IWLTenantDomainService, WLTenantDomainService>();
builder.Services.AddScoped<IWLMasterCmsService, WLMasterCmsService>();

builder.Services.AddScoped<IInquiryGeneralService, InquiryGeneralService>();
builder.Services.AddScoped<IInquiryAgentService, InquiryAgentService>();
builder.Services.AddScoped<IInquiryWhiteLabelService, InquiryWhiteLabelService>();
builder.Services.AddScoped<IInquiryDistributorService, InquiryDistributorService>();
builder.Services.AddScoped<IInquiryCareerService, InquiryCareerService>();
builder.Services.AddScoped<IWLAdminAuthLogsService, WLAdminAuthLogsService>();
builder.Services.AddScoped<IPagedService<InquiryGeneralDto>, InquiryGeneralService>();
builder.Services.AddScoped<ISupportTicketCategoryService, SupportTicketCategoryService>();
builder.Services.AddScoped<IWLTestimonialService, WLTestimonialService>();
builder.Services.AddScoped<ISupportTicketHeaderService, SupportTicketHeaderService>();
builder.Services.AddScoped<ILoginAttemptService, LoginAttemptService>();
builder.Services.AddScoped<IPasswordPolicyService, PasswordPolicyService>();
builder.Services.AddScoped<ICredentialsWhatsappServices, CredentialsWhatsappServices>();
builder.Services.AddScoped<ICredentialsSMSGatewayServices, CredentialsSMSGatewayServices>();
builder.Services.AddScoped<IWLMasterPermissionGroupService, WLMasterPermissionGroupService>();
builder.Services.AddScoped<IApiEndpointService, ApiEndpointService>();
builder.Services.AddScoped<IMasterFaqSubCategoryService, MasterFaqSubCategoryService>();
builder.Services.AddScoped<IApiXVersionService, ApiXVersionService>();
builder.Services.AddScoped<IApixHeaderService, ApixHeaderService>();
builder.Services.AddScoped<IRequestSchemaService, RequestSchemaService>();
builder.Services.AddScoped<IReqResSchemaFieldsService, ReqResSchemaFieldsService>();
builder.Services.AddScoped<IResponseSchemaService, ResponseSchemaService>();
builder.Services.AddScoped<IApiXCodeMapperService, ApiXCodeMapperService>();
builder.Services.AddScoped<IApiXCodeExampleService, ApiXCodeExampleService>();
builder.Services.AddScoped<IMasterProgrammingLanguageService, MasterProgrammingLanguageService>();
builder.Services.AddScoped<IApiXLanguageContentService, ApiXLanguageContentService>();
builder.Services.AddScoped<IEncryptionService, EncryptionService>();
builder.Services.AddScoped<IPaymentGatewayCredentialService, PaymentGatewayCredentialService>();
builder.Services.AddScoped<IMasterOfferService, MasterOfferService>();
// repositories
builder.Services.AddScoped<IMasterPolicyService, MasterPolicyService>();
builder.Services.AddScoped<IHmacService, HmacService>();
builder.Services.AddScoped<IMasterVerificationFeeService, MasterVerificationFeeService>();
builder.Services.AddScoped<IWebsiteFaqCategoryService, WebsiteFaqCategoryService>();
builder.Services.AddScoped<IWebsiteMasterFaqService, WebsiteMasterFaqService>();
builder.Services.AddScoped<IMasterSkillsService, MasterSkillsService>();
builder.Services.AddScoped<IMasterJobTypeService, MasterJobTypeService>();
builder.Services.AddScoped<INotificationOrchestrator, NotificationOrchestrator>();
builder.Services.AddScoped<INotificationChannelSender, EmailSender>();
builder.Services.AddScoped<IAppSettingService, AppSettingService>();
//builder.Services.AddScoped<IMasterCustomerService, MasterCustomerService>();

// 
// 
#endregion

#region DI : Repositories
builder.Services.AddScoped<IMasterRepository<Website_IndustryCases>, MasterRepositoryBase<Website_IndustryCases>>();
builder.Services.AddScoped<IMasterRepository<Website_MasterIndustry>, MasterRepositoryBase<Website_MasterIndustry>>();
builder.Services.AddScoped<IMasterRepository<Website_IndustryPoints>, MasterRepositoryBase<Website_IndustryPoints>>();
builder.Services.AddScoped<IMasterRepository<Website_VerificationServiceCategory>, MasterRepositoryBase<Website_VerificationServiceCategory>>();
builder.Services.AddScoped<IMasterRepository<Website_VerificationService>, WebsiteCategoryServiceRepository>();

builder.Services.AddScoped<IManageApiRepository, ManageApiRepository>();
builder.Services.AddScoped<ICommonRepository, CommonRepository>();
builder.Services.AddScoped<IMasterPricingRepository, MasterPricingRepository>();
builder.Services.AddScoped<IMasterRepository<SavedNews>, MasterRepositoryBase<SavedNews>>();
builder.Services.AddScoped<IMasterRepository<MasterBusinessIndustry>, MasterRepositoryBase<MasterBusinessIndustry>>();
builder.Services.AddScoped<IMasterRepository<AppSetting>, MasterRepositoryBase<AppSetting>>();
builder.Services.AddScoped<IWLLoginAttemptRepository, WLLoginAttemptRepository>();
builder.Services.AddScoped<IMasterRepository<SavedNews>, MasterRepositoryBase<SavedNews>>();
builder.Services.AddScoped<IVerificationRequestLogRepository, VerificationRequestLogRepository>();
builder.Services.AddScoped<IMasterRepository<ApiXCategory>, MasterApiXCategoryRepository>();
builder.Services.AddScoped<IWLMasterPathPermissionRepository, WLMasterPathPermissionRepository>();
builder.Services.AddScoped<IWLActionLogsRepository, WLActionLogsRepository>();
builder.Services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
builder.Services.AddScoped<IMasterRepository<WL_Master_PathPermission>, WLMasterPathPermissionRepository>();  // ✅ ADD THIS
builder.Services.AddScoped<IMasterRepository<WL_Master_Path>, MasterRepositoryBase<WL_Master_Path>>();
builder.Services.AddScoped<IMasterRepository<Master_ProgrammingLanguage>, MasterRepositoryBase<Master_ProgrammingLanguage>>();
builder.Services.AddScoped<IMasterRepository<WL_Master_Path>, MasterRepositoryBase<WL_Master_Path>>();
builder.Services.AddScoped<IMasterRepository<Credential_SMS_Gateway>, MasterRepositoryBase<Credential_SMS_Gateway>>();
builder.Services.AddScoped<IMasterRepository<Master_Gender>, MasterRepositoryBase<Master_Gender>>();
builder.Services.AddScoped<IMasterRepository<Master_EmailCredential>, MasterRepositoryBase<Master_EmailCredential>>();
builder.Services.AddScoped<IMasterRepository<Master_EmailTemplate>, MasterRepositoryBase<Master_EmailTemplate>>();
builder.Services.AddScoped<IMasterRepository<Master_Honorific>, MasterRepositoryBase<Master_Honorific>>();
builder.Services.AddScoped<IMasterRepository<Api_Category>, MasterRepositoryBase<Api_Category>>();
builder.Services.AddScoped<IMasterRepository<ApiXStatusCodes>, MasterRepositoryBase<ApiXStatusCodes>>();
builder.Services.AddScoped<IMasterRepository<Master_Designation>, MasterRepositoryBase<Master_Designation>>();
builder.Services.AddScoped<IMasterRepository<Master_Industry>, MasterRepositoryBase<Master_Industry>>();
builder.Services.AddScoped<IMasterRepository<Master_Country>, MasterRepositoryBase<Master_Country>>();
builder.Services.AddScoped<IMasterRepository<WL_MasterPermission>, WLMasterPermissionRepository>();
builder.Services.AddScoped<IMasterRepository<Master_State>, WebsiteCatServiceRepository>();
builder.Services.AddScoped<IMasterRepository<Master_City>, MasterCityRepository>();
builder.Services.AddScoped<IMasterRepository<Master_Department>, MasterRepositoryBase<Master_Department>>();
builder.Services.AddScoped<IMasterRepository<Master_Permission>, MasterPermissionRepository>();
builder.Services.AddScoped<IMasterRepository<Master_Roles>, MasterRepositoryBase<Master_Roles>>();
builder.Services.AddScoped<IMasterMenuRepository, MasterMenuRepository>();
builder.Services.AddScoped<IWLMasterMenuRepository, WLMasterMenuRepository>();
builder.Services.AddScoped<IMasterRepository<Provider_Apis>, ProviderApisRepository>();
builder.Services.AddScoped<IMasterRepository<Master_Path>, MasterRepositoryBase<Master_Path>>();
builder.Services.AddScoped<IMasterRepository<Api_Components>, MasterRepositoryBase<Api_Components>>();
builder.Services.AddScoped<IMasterRepository<TrustedPartners>, MasterRepositoryBase<TrustedPartners>>();
builder.Services.AddScoped<IMasterRepository<Credential_Whatsapp>, MasterRepositoryBase<Credential_Whatsapp>>();
//builder.Services.AddScoped<IMasterRepository<Master_PathPermission>, MasterRepositoryBase<Master_PathPermission>>();
//builder.Services.AddScoped<IMasterRepository<Menu_RolePermission>, MasterRepositoryBase<Menu_RolePermission>>();
builder.Services.AddScoped<IMasterRepository<Menu_RolePermission>, MenuRolePermissionRepository>();
builder.Services.AddScoped<IMasterRepository<Master_PermissionGroup>, MasterRepositoryBase<Master_PermissionGroup>>();
// Register the concrete repo so your override is used for paged queries
builder.Services.AddScoped<IMasterRepository<Master_PathPermission>, MasterPathPermissionRepository>();
// Register the concrete repo so your override is used for paged queries
builder.Services.AddScoped<ITenantService, TenantService>();

builder.Services.AddScoped<IMasterRepository<Master_Employee>, MasterEmployeeRepository>();
builder.Services.AddScoped<IMasterEmployeeRepository, MasterEmployeeRepository>();
//builder.Services.AddScoped<IMasterRepository<Master_Nomenclature>, MasterRepositoryBase<Master_Nomenclature>>();
builder.Services.AddScoped<IMasterRepository<Master_Nomenclature>, MasterNomenClatureRepository>();
builder.Services.AddScoped<IMasterNomenClatureRepository, MasterNomenClatureRepository>();
builder.Services.AddScoped<IPasswordResetRepository, PasswordResetRepository>();
builder.Services.AddScoped<ILoginHistoryRepository, LoginHistoryRepository>();
builder.Services.AddScoped<IMasterRepository<Api_Provider>, MasterRepositoryBase<Api_Provider>>();
builder.Services.AddScoped<IMasterRepository<Api_ProviderMapping>, MasterRepositoryBase<Api_ProviderMapping>>();
builder.Services.AddScoped<IMasterRepository<Master_Api>, MasterApiRepository>();
builder.Services.AddScoped<IMasterRepository<Api_ProviderMapping>, ApiProviderMappingRepository>();
builder.Services.AddScoped<IWLAdminAuthLogsRepository, WLAdminAuthLogsRepository>();

builder.Services.AddScoped<ILoginLogoutLogsRepository, LoginLogoutLogsRepository>();
builder.Services.AddScoped<ILoginAttemptRepository, LoginAttemptRepository>();
builder.Services.AddScoped<IAdminAuthLogsRepository, AdminAuthLogsRepository>();
builder.Services.AddScoped<IMasterRepository<Master_DosDonts>, MasterRepositoryBase<Master_DosDonts>>();

builder.Services.AddScoped<IMasterRepository<Master_Api>, MasterApiRepository>();
builder.Services.AddScoped<IMasterEmailCredentialRepository, MasterEmailCredentialRepository>();
builder.Services.AddScoped<IMasterEmailTemplateRepository, MasterEmailTemplateRepository>();

builder.Services.AddScoped<IMasterRepository<Api_ProviderComponentMapping>, ApiProviderComponentMappingRepository>();
builder.Services.AddScoped<IMasterRepository<Master_Testimonial>, MasterRepositoryBase<Master_Testimonial>>();
builder.Services.AddScoped<IMasterRepository<Clients>, MasterRepositoryBase<Clients>>();
builder.Services.AddScoped<IMasterRepository<Master_Career>, MasterCareerRepository>();
builder.Services.AddScoped<IMasterRepository<Certifications>, MasterRepositoryBase<Certifications>>();
builder.Services.AddScoped<IMasterRepository<OurTeam>, MasterRepositoryBase<OurTeam>>();
//builder.Services.AddScoped<IMasterRepository<Master_FAQ>, MasterRepositoryBase<Master_FAQ>>();

builder.Services.AddScoped<IMasterRepository<Master_FAQCategory>, MasterRepositoryBase<Master_FAQCategory>>();
builder.Services.AddScoped<IMasterRepository<Service_Category>, MasterRepositoryBase<Service_Category>>();
builder.Services.AddScoped<IMasterRepository<Master_FAQ>, MasterFAQRepository>();
builder.Services.AddScoped<IMasterRepository<Knowledge_Hub_Category>, MasterRepositoryBase<Knowledge_Hub_Category>>();
builder.Services.AddScoped<IMasterRepository<Knowledge_Hub>, KnowledgeHubRepository>();
builder.Services.AddScoped<IMasterRepository<News>, NewsRepository>();
builder.Services.AddScoped<IMasterRepository<News_Category>, MasterRepositoryBase<News_Category>>();

builder.Services.AddScoped<IMasterRepository<Master_Blog>, MasterBlogRepository>();
builder.Services.AddScoped<IMasterRepository<Master_BlogCategory>, MasterRepositoryBase<Master_BlogCategory>>();
builder.Services.AddScoped<IMasterRepository<Master_SocialMedia>, MasterRepositoryBase<Master_SocialMedia>>();

builder.Services.AddScoped<IMasterRepository<ApiInfoSection>, ApiInfoSectionRepository>();
builder.Services.AddScoped<IMasterRepository<ApiInfoFields>, ApiInfoFieldsRepository>();

builder.Services.AddScoped<ITenantRepository, TenantRepository>();
builder.Services.AddScoped<IMasterRepository<Tenant>, TenantRepository>();
builder.Services.AddScoped<IMasterRepository<Master_Banner>, MasterRepositoryBase<Master_Banner>>();
//builder.Services.AddScoped<IMasterRepository<Master_Banner>, MasterRepositoryBase<Master_Banner>>();
builder.Services.AddScoped<IMasterRepository<Master_CMS>, MasterRepositoryBase<Master_CMS>>();

builder.Services.AddScoped<IMasterCompanyBasicDataRepository, MasterComapnyBasicDataRepository>();
builder.Services.AddScoped<IActionLogsRepository, ActionLogsRepository>();
builder.Services.AddScoped<IMasterRepository<Tenant>, MasterRepositoryBase<Tenant>>();
//builder.Services.AddScoped<IMasterRepository<WL_MasterSocialMedia>, MasterRepositoryBase<WL_MasterSocialMedia>>();
builder.Services.AddScoped<IMasterRepository<WL_MasterSocialMedia>, WLSocialMediaRepository>();
builder.Services.AddScoped<IMasterRepository<WL_MasterBanner>, WLMasterBannerRepository>();
builder.Services.AddScoped<IMasterRepository<WL_MasterCompanyBasicData>, WLMasterBasicCompanyRepository>();
builder.Services.AddScoped<IMasterRepository<WL_MasterCMS>, WLMasterCmsRepository>();

// Register custom repo so its GetPagedAsync override runs
builder.Services.AddScoped<IMasterRepository<TenantDomain>, WLTenantDomainRepository>();
//builder.Services.AddScoped<IMasterRepository<TenantDomain>, MasterRepositoryBase<TenantDomain>>();
builder.Services.AddScoped<IActivityLogsRepository, ActivityLogsRepository>();

builder.Services.AddScoped<IMasterRepository<Master_Document>, MasterRepositoryBase<Master_Document>>();
//builder.Services.AddScoped<IMasterRepository<WL_Clients>, MasterRepositoryBase<WL_Clients>>();
builder.Services.AddScoped<IMasterRepository<WL_Clients>, WLClientsRepository>();
builder.Services.AddScoped<IMasterRepository<Master_FinancialYear>, MasterRepositoryBase<Master_FinancialYear>>();
builder.Services.AddScoped<ICustomerConsentRepository, CustomerConsentRepository>();
builder.Services.AddScoped<IMasterRepository<Support_TicketCategory>, SupportTicketCategoryRepository>();
//builder.Services.AddScoped<IMasterRepository<Master_Employee>, MasterEmployeeRepository>();
/*builder.Services.AddScoped<IMasterRepository<LoginAttempts>, MasterRepositoryBase<LoginAttempts>>();*/

builder.Services.AddScoped<IInquiryGeneralRepository, InquiryGeneralRepository>();
builder.Services.AddScoped<IInquiryAgentRepository, InquiryAgentRepository>();
builder.Services.AddScoped<IInquiryWhiteLabelRepository, InquiryWhiteLabelRepository>();
builder.Services.AddScoped<IInquiryDistributorRepository, InquiryDistributorRepository>();
builder.Services.AddScoped<IInquiryCareerRepository, InquiryCareerRepository>();

builder.Services.AddScoped<IMasterRepository<WL_MasterPermissionGroup>, MasterRepositoryBase<WL_MasterPermissionGroup>>();

builder.Services.AddScoped<IMasterRepository<Support_TicketHeader>, SupportTicketHeaderRepository>();
builder.Services.AddScoped<IMasterRepository<Support_TicketLine>, MasterRepositoryBase<Support_TicketLine>>();
builder.Services.AddScoped<IMasterRepository<Support_TicketLineAttachment>, MasterRepositoryBase<Support_TicketLineAttachment>>();
// register specialized repository so its GetPagedAsync (that left-joins Tenant) runs
builder.Services.AddScoped<IMasterRepository<WL_MasterTestimonial>, WLTestimonialRepository>();
builder.Services.AddScoped<IPasswordPolicyRepository, PasswordPolicyRepository>();
builder.Services.AddScoped<IMasterRepository<Password_Policy>, MasterRepositoryBase<Password_Policy>>();
builder.Services.AddScoped<IMasterRepository<Master_Customer>, MasterCustomerRepository>();
builder.Services.AddScoped<IMasterCustomerRepository, MasterCustomerRepository>();
builder.Services.AddScoped<IMasterRepository<Api_Endpoint>, ApiEndpointRepository>();

builder.Services.AddScoped<IMasterRepository<Master_FAQSubCategory>, MasterFaqSubCategoryRepository>();
builder.Services.AddScoped<IMasterRepository<ApiXVersion>, MasterRepositoryBase<ApiXVersion>>();
// Register repository for ApiXHeaders required by ApiXVersionService
builder.Services.AddScoped<IMasterRepository<ApiXHeaders>, MasterRepositoryBase<ApiXHeaders>>();
builder.Services.AddScoped<IMasterRepository<RequestSchema>, MasterRepositoryBase<RequestSchema>>();
builder.Services.AddScoped<IMasterRepository<ReqResSchemaFields>, MasterRepositoryBase<ReqResSchemaFields>>();
builder.Services.AddScoped<IMasterRepository<ResponseSchema>, MasterRepositoryBase<ResponseSchema>>();
builder.Services.AddScoped<IMasterRepository<ApiXCodeMapper>, MasterRepositoryBase<ApiXCodeMapper>>();
builder.Services.AddScoped<IMasterRepository<ApiXCodeExample>, MasterRepositoryBase<ApiXCodeExample>>();
builder.Services.AddScoped<IMasterPolicyRepository, MasterPolicyRepository>();
builder.Services.AddScoped<IMasterRepository<Master_Policy>, MasterRepositoryBase<Master_Policy>>();

builder.Services.AddScoped<IMasterRepository<ApiXLanguageContent>, MasterRepositoryBase<ApiXLanguageContent>>();
builder.Services.AddScoped<IMasterRepository<PaymentGatewayCredential>, MasterRepositoryBase<PaymentGatewayCredential>>();
builder.Services.AddScoped<IMasterRepository<Master_Offer>, MasterRepositoryBase<Master_Offer>>();
builder.Services.AddScoped<IMasterRepository<Master_VerificationFee>, MasterRepositoryBase<Master_VerificationFee>>();
builder.Services.AddScoped<IMasterRepository<Website_FAQCategory>, MasterRepositoryBase<Website_FAQCategory>>();
builder.Services.AddScoped<IMasterRepository<Website_MasterFAQ>, MasterWebsiteFAQRepository>();
builder.Services.AddScoped<IMasterRepository<Master_Skills>, MasterRepositoryBase<Master_Skills>>();
builder.Services.AddScoped<IMasterRepository<Master_JobType>, MasterRepositoryBase<Master_JobType>>();
builder.Services.AddScoped<IUserNotificationRepository, UserNotificationRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<ITemplateRenderer, TemplateRenderer>();
builder.Services.AddScoped<IAppSettingRepository, AppSettingRepository>();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

#region Tenant Setup
app.Use(async (context, next) =>
{
    const string tenantHeader = "X-Tenant";

    if (string.IsNullOrWhiteSpace(context.Request.Headers[tenantHeader]))
    {
        var host = context.Request.Host.Host;
        if (!string.IsNullOrWhiteSpace(host))
        {
            context.Request.Headers[tenantHeader] = host;
        }
    }

    await next();
});

app.UseMultiTenant();
#endregion

app.UseAuthentication();
app.UseAuthorization();


// Add cache control for authenticated pages BEFORE route mapping
app.Use(async (context, next) =>
{
    // If user is authenticated, apply no-cache headers
    if (context.User?.Identity?.IsAuthenticated == true)
    {
        context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate, max-age=0, private";
        context.Response.Headers["Pragma"] = "no-cache";
        context.Response.Headers["Expires"] = "-1";
        context.Response.Headers["Surrogate-Control"] = "no-store";
    }

    await next();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");


//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
