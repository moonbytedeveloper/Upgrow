using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Upgrow.Application.DTO.ActionLogs;
using Upgrow.Application.DTO.CustomerPanel;
using Upgrow.Application.DTO.CustomerPanel.QueryResults;
using Upgrow.Application.IServices;
using Upgrow.Application.IServices.Master;
using Upgrow.Application.Utilities;
using Upgrow.Domain;
using Upgrow.Domain.Common;
using Upgrow.Domain.Entities;
using Upgrow.Domain.Entities.Auth;
using Upgrow.Domain.Entities.Inquiry;
using Upgrow.Domain.Entities.Registration;
using Upgrow.Domain.Entities.Website;
using Upgrow.Domain.Entities.WL;
using Upgrow.Domain.Entities.WL.Master;
using static Upgrow.Application.Constants;

namespace Upgrow.Infrastructure
{
    public class AppDbContext : DbContext
    {
        #region Tenant and Log Setup
        private readonly ITenantSetupService _tenantSetupService;
        private readonly IHttpContextAccessor? _httpContextAccessor;
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        private const string PlatformIdentifier = "authenticateindia";  // WARNING: NEVER CHANGE THIS VALUE as it's used to determine if an ActionLog should be stored in master ActionLogs table or tenant-specific WL_ActionLogs table
        public int CurrentTenantId => _tenantSetupService.TenantId;
        public string CurrentTenantIdentifier => _tenantSetupService.Identifier;

        private static readonly HashSet<string> _auditIgnoreEntities = new(StringComparer.OrdinalIgnoreCase)
        {
            nameof(ActionLogs),
            nameof(ActivityLogs),
            nameof(AdminAuthLogs),
            nameof(LoginAttempts),
            nameof(WL_ActionLogs),
            nameof(WL_AdminAuthLogs) // <-- ADD THIS
        };

        public static ISet<string> AuditIgnoreEntities => _auditIgnoreEntities;

        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            ITenantSetupService tenantSetupService,
            IDbContextFactory<AppDbContext> contextFactory,
            IHttpContextAccessor? httpContextAccessor = null)
            : base(options)
        {
            _contextFactory = contextFactory;
            _tenantSetupService = tenantSetupService;
            _httpContextAccessor = httpContextAccessor;
        }

        private void SetTenantFilter<TEntity>(ModelBuilder builder)
    where TEntity : class, ITenantEntity
        {
            builder.Entity<TEntity>()
                .HasQueryFilter(e => CurrentTenantIdentifier == PlatformIdentifier || e.TenantId == CurrentTenantId);
        }

        private void ApplyTenantFilter(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(TenantEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(AppDbContext)
                        .GetMethod(nameof(SetTenantFilter), BindingFlags.NonPublic | BindingFlags.Instance)
                        ?.MakeGenericMethod(entityType.ClrType);

                    method?.Invoke(this, new object[] { modelBuilder });
                }
            }
        }

        public override int SaveChanges()
        {
            ApplyTenantIds();
            GenerateRecordHashes();
            try
            {
                // We need to separate and handle creation of tenant-specific WL_ActionLogs
                var auditLogs =  CreateAuditLogs();
                if (auditLogs.Count > 0)
                {
                    // If it's a tenant log, route to WL_ActionLogs, otherwise master ActionLogs
                    if (CurrentTenantIdentifier != PlatformIdentifier)
                    {
                        var wlLogs = auditLogs.Select(log => new WL_ActionLogs
                        {
                            Payload = log.Payload,
                            PreviousHash = log.PreviousHash,
                            CurrentHash = log.CurrentHash,
                            DigitalSignature = log.DigitalSignature,
                            TenantId = CurrentTenantId,
                            RecordHash = log.RecordHash
                        }).ToList();

                        WL_ActionLogs.AddRange(wlLogs);
                    }
                    else
                    {
                        ActionLogs.AddRange(auditLogs);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyTenantIds();
            GenerateRecordHashes();
            try
            {
            var auditLogs = CreateAuditLogs();
            if (auditLogs.Count > 0)
            {
                if (CurrentTenantIdentifier != PlatformIdentifier)
                {
                    var wlLogs = auditLogs.Select(log => new WL_ActionLogs
                    {
                        Payload = log.Payload,
                        PreviousHash = log.PreviousHash,
                        CurrentHash = log.CurrentHash,
                        DigitalSignature = log.DigitalSignature,
                        TenantId = CurrentTenantId,
                        RecordHash = log.RecordHash
                    }).ToList();

                    WL_ActionLogs.AddRange(wlLogs);
                }
                else
                {
                    ActionLogs.AddRange(auditLogs);
                }
            }
            }
            catch (Exception ex) 
            {
                // Log the exception or handle it as needed

            }
            return base.SaveChangesAsync(cancellationToken);
        }

        //private void GenerateRecordHashes()
        //{
        //    foreach (var entry in ChangeTracker.Entries()
        //                 .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
        //    {
        //        var entity = entry.Entity;
        //        var jsonPayload = JsonSerializer.Serialize(entity, new JsonSerializerOptions
        //        {
        //            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        //        });

        //        var recordHash = ActionLogHashUtility.ComputeCurrentHash(jsonPayload);

        //        var recordHashProperty = entity.GetType().GetProperty("RecordHash");
        //        if (recordHashProperty != null && recordHashProperty.CanWrite)
        //        {
        //            recordHashProperty.SetValue(entity, recordHash);
        //        }
        //    }
        //}
        private void GenerateRecordHashes()
        {
            foreach (var entry in ChangeTracker.Entries()
                     .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                var values = new Dictionary<string, object?>();

                foreach (var property in entry.Properties)
                {
                    values[property.Metadata.Name] = property.CurrentValue;
                }

                var jsonPayload = JsonSerializer.Serialize(values, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });

                var recordHash = ActionLogHashUtility.ComputeCurrentHash(jsonPayload);

                var recordHashProperty = entry.Entity.GetType().GetProperty("RecordHash");
                if (recordHashProperty != null && recordHashProperty.CanWrite)
                {
                    recordHashProperty.SetValue(entry.Entity, recordHash);
                }
            }
        }

        private void ApplyTenantIds()
        {
            foreach (var entry in ChangeTracker.Entries<ITenantEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.TenantId = CurrentTenantId;
                }
            }
        }

        private List<ActionLog> CreateAuditLogs()
        {

            try
            {
                var logs = new List<ActionLog>();
                var now = DateTimeOffset.UtcNow;  // Instead of DateTime.UtcNow

                var httpContext = _httpContextAccessor?.HttpContext;
                var userUuid = httpContext?.User?.FindFirst(ClaimConstants.UUID)?.Value;
                var ipAddress = httpContext?.Connection?.RemoteIpAddress?.ToString();

                foreach (var entry in ChangeTracker.Entries()
                             .Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted))
                {
                    if (!ShouldAuditEntity(entry))
                    {
                        continue;
                    }

                    var oldValues = new Dictionary<string, object?>();
                    var newValues = new Dictionary<string, object?>();

                    foreach (var property in entry.Properties)
                    {
                        if (property.Metadata.IsShadowProperty())
                        {
                            continue;
                        }

                        var name = property.Metadata.Name;

                        if (entry.State == EntityState.Added)
                        {
                            newValues[name] = property.CurrentValue;
                            continue;
                        }

                        if (entry.State == EntityState.Deleted)
                        {
                            oldValues[name] = property.OriginalValue;
                            continue;
                        }

                        // Modified: store ONLY truly changed fields
                        if (entry.State == EntityState.Modified && property.IsModified)
                        {
                            var original = property.OriginalValue;
                            var current = property.CurrentValue;

                            if (!HasValueChanged(original, current))
                            {
                                continue;
                            }

                            oldValues[name] = original;
                            newValues[name] = current;
                        }
                    }

                    // attach explicit password change reason (set by controller) to employee password update logs
                    if (entry.State == EntityState.Modified
                        && ((entry.Entity is Master_Employee))
                        && newValues.ContainsKey("Password"))
                    {
                        var reason = _httpContextAccessor?.HttpContext?.Items["PasswordChangeReason"]?.ToString();
                        if (!string.IsNullOrWhiteSpace(reason))
                        {
                            newValues["PasswordChangeReason"] = reason;
                        }
                    }

                    var actionType = entry.State switch
                    {
                        EntityState.Added => "Create",
                        EntityState.Deleted => "Delete",
                        EntityState.Modified when ((entry.Entity is Master_Employee)) && newValues.ContainsKey("Password") => "Password Changed",
                        _ => "Update"
                    };

                    // If no real field changed, skip creating update log row
                    if (entry.State == EntityState.Modified && oldValues.Count == 0 && newValues.Count == 0)
                    {
                        continue;
                    }

                    var data = new ActionLogDto
                    {
                        UserUUID = userUuid,
                        EntityName = entry.Metadata.ClrType.Name,
                        EntityUUID = GetEntityUuid(entry.Entity),
                        ActionType = actionType,
                        OldValues = oldValues.Count == 0 ? null : JsonSerializer.Serialize(oldValues),
                        NewValues = newValues.Count == 0 ? null : JsonSerializer.Serialize(newValues),
                        IPAddress = ipAddress,
                        CreatedAt = now,
                        RootEntityName = null,
                        RootEntityUUID = null
                    };

                    var json = JsonSerializer.Serialize(data);

                    var log = new ActionLog
                    {
                        Payload = json,
                        PreviousHash = GetLastCurrentHash(),
                        CurrentHash = ActionLogHashUtility.ComputeCurrentHash(json),
                        RecordHash = entry.Entity.GetType().GetProperty("RecordHash")?.GetValue(entry.Entity) as byte[] // Assign RecordHash separately

                    };

                    var privateKeyPath = KeyPathResolver.GetPrivateKeyPath();
                    try
                    {
                        log.DigitalSignature = ActionLogHashUtility.ComputeDigitalSignature(log.CurrentHash, privateKeyPath);
                    }
                    catch
                    {
                    }

                    logs.Add(log);
                }

                return logs;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private byte[]? GetLastCurrentHash()
        {
            if (CurrentTenantIdentifier != PlatformIdentifier)
            {
                return WL_ActionLogs
                    .AsNoTracking()
                    .Where(x => x.TenantId == CurrentTenantId)
                    .OrderByDescending(x => x.Id)
                    .Select(x => x.CurrentHash)
                    .FirstOrDefault();
            }

            return ActionLogs
                .AsNoTracking()
                .OrderByDescending(x => x.Id)
                .Select(x => x.CurrentHash)
                .FirstOrDefault();
        }

        private static bool HasValueChanged(object? original, object? current)
        {
            if (ReferenceEquals(original, current))
            {
                return false;
            }

            if (original is null || current is null)
            {
                return true;
            }

            if (original is byte[] originalBytes && current is byte[] currentBytes)
            {
                return !originalBytes.SequenceEqual(currentBytes);
            }

            return !Equals(original, current);
        }

        private static bool ShouldAuditEntity(EntityEntry entry)
        {
            if (entry.Metadata.IsOwned())
            {
                return false;
            }

            var entityName = entry.Metadata.ClrType.Name;
            return !_auditIgnoreEntities.Contains(entityName);
        }

        private static string? GetEntityUuid(object entity)
        {
            if (entity is TenantEntity baseEntity)
            {
                return baseEntity.UUID;
            }

            return entity.GetType().GetProperty("UUID")?.GetValue(entity)?.ToString();
        }
        #endregion

        //public int CurrentTenantId => _tenantService.TenantId;
        public DbSet<CustomerBusinessSession> CustomerBusinessSession { get; set; }
        public DbSet<Master_BusinessType> Master_BusinessType { get; set; }

        public DbSet<CustomerOrganization> CustomerOrganization { get; set; }
        public DbSet<TransactionBridgeLog> TransactionBridgeLog { get; set; }
        public DbSet<MasterSMSCredential> MasterSMSCredential { get; set; }

        public DbSet<MasterSMSTemplate> MasterSMSTemplate { get; set; }
        public DbSet<CustomerEmailVerification> CustomerEmailVerification { get; set; }
        public DbSet<Website_VerificationServiceCategory> Website_VerificationServiceCategory { get; set; }
        public DbSet<Website_VerificationService> Website_VerificationService { get; set; }

        public DbSet<MasterDosDontsDocument> MasterDosDontsDocument { get; set; }
        public DbSet<CustomerConsentDosDonts> CustomerConsentDosDonts { get; set; }
        public DbSet<MasterBusinessIndustry> MasterBusinessIndustry { get; set; }
        public DbSet<TransactionRzpPGRecord> TransactionRzpPGRecords { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<TransactionDetail> TransactionDetails { get; set; }

        public DbSet<TransactionConsent> TransactionConsents { get; set; }

        public DbSet<CustomerCreditMaster> CustomerCreditMasters { get; set; }

        public DbSet<CustomerCreditLedger> CustomerCreditLedgers { get; set; }

        public DbSet<CustomerDebitLedger> CustomerDebitLedgers { get; set; }
        public DbSet<Master_Cart> Master_Carts { get; set;}
        public DbSet<Website_IndustryCases> Website_IndustryCases { get; set; }
        public DbSet<Website_IndustryPoints> Website_IndustryPoints { get; set; }

        public DbSet<AppSetting> AppSetting { get; set; }
        public DbSet<CartDetail> CartDetails { get; set; }
        public DbSet<Website_MasterIndustry> Website_MasterIndustry { get; set; }
        public DbSet<CustomerVideoKyc> CustomerVideoKyc { get; set; }
        public DbSet<ApiInfoFields> ApiInfoFields { get; set; }
        public DbSet<ApiInfoSection> ApiInfoSections { get; set; }

        public DbSet<Master_VerificationFee> Master_VerificationFee { get; set; }
        public DbSet<CustomerAadhaarSession> CustomerAadhaarSession { get; set; }
        public DbSet<CustomerRegDocument> CustomerRegDocument { get; set; }
        public DbSet<CustomerOtp> CustomerOtp { get; set; }
        public DbSet<RequestSchema> RequestSchema { get; set; }
        public DbSet<ReqResSchemaFields> ReqResSchemaFields { get; set; }
 
        public DbSet<Master_ProgrammingLanguage> Master_ProgrammingLanguage { get; set; }
        public DbSet<ApiXLanguageContent> ApiXLanguageContent { get; set; }
        public DbSet<ApiXCodeMapper> ApiXCodeMapper { get; set; }
        public DbSet<ApiXCodeExample> ApiXCodeExample { get; set; }
        public DbSet<ResponseSchema> ResponseSchema { get; set; }
        public DbSet<ApiXHeaders> ApiXHeaders { get; set; }
        public DbSet<ApiXStatusCodes> ApiXStatusCodes { get; set; }
        public DbSet<ApiXCategory> ApiXCategory { get; set; }
        public DbSet<RefreshTokens> RefreshTokens { get; set; }
        public DbSet<ApiXVersion> ApiXVersion { get; set; }
        public DbSet<WL_LoginAttempts> WL_LoginAttempts { get; set; }
        public DbSet<WL_MasterPermission> WL_MasterPermission { get; set; }
        public DbSet<WL_Master_Path> WL_Master_Path { get; set; }
        public DbSet<WL_Master_PathPermission> WL_Master_PathPermission { get; set; }
        public DbSet<WL_MasterEmailCredential> WL_MasterEmailCredential { get; set; }
        // Existing DbSets...
        public DbSet<WL_ActionLogs> WL_ActionLogs { get; set; }
        public DbSet<WL_MasterMenu> WL_MasterMenu { get; set; }
        public DbSet<WL_MasterCity> WL_MasterCity { get; set; }
        public DbSet<WL_MasterState> WL_MasterState { get; set; }
        public DbSet<WL_MasterCountry> WL_MasterCountry { get; set; }
        public DbSet<LoginAttempts> LoginAttempts { get; set; }
        public DbSet<AdminAuthLogs> AdminAuthLogs { get; set; }
        public DbSet<ActionLog> ActionLogs { get; set; }
        public DbSet<Configuration> Configuration { get; set; }
        public DbSet<Master_Testimonial> Master_Testimonial { get; set; }
        public DbSet<Master_Banner> Master_Banner { get; set; }
        public DbSet<Master_Honorific> Master_Honorific { get; set; }
        public DbSet<Master_SocialMedia> Master_SocialMedia { get; set; }
        public DbSet<Provider_Apis> ProviderApis { get; set; }
        public DbSet<Master_Gender> Master_Gender { get; set; }
        public DbSet<Master_Offer> Master_Offer { get; set; }
        public DbSet<Master_Nomenclature> Master_Nomenclature { get; set; }
        public DbSet<Master_CMS> Master_CMS { get; set; }
        public DbSet<Master_Blog> Master_Blog { get; set; }
        public DbSet<Master_BlogCategory> Master_BlogCategory { get; set; }
        public DbSet<Master_PermissionGroup> Master_PermissionGroup { get; set; }
        public DbSet<TrustedPartners> TrustedPartner { get; set; }
        public DbSet<Master_Brand> Master_Brand { get; set; }

        public DbSet<Master_EmailCredential> Master_EmailCredential { get; set; }
        public DbSet<Master_EmailTemplate> Master_EmailTemplate { get; set; }
        public DbSet<Master_CompanyBasicData> Master_CompanyBasicData { get; set; }
        public DbSet<Inquiry_General> Inquiry_General { get; set; }
        public DbSet<Inquiry_Agent> Inquiry_Agent { get; set; }
        public DbSet<Inquiry_WhiteLabel> Inquiry_WhiteLabel { get; set; }
        public DbSet<Inquiry_Distributor> Inquiry_Distributor { get; set; }
        public DbSet<Inquiry_Career> Inquiry_Career { get; set; }
        public DbSet<Inquiry_Contact> inquiry_Contact { get; set; }
        public DbSet<Inquiry_Partner> Inquiry_Partner { get; set; }
        public DbSet<NotificationSound> NotificationSound { get; set; }
        public DbSet<CustomerNotificationPreference> CustomerNotificationPreference { get; set; }
        public DbSet<Master_Department> Master_Department { get; set; }
        public DbSet<Master_Document> Master_Document { get; set; }
        public DbSet<Master_FAQ> Master_FAQ { get; set; }
        public DbSet<Master_Country> Master_Country { get; set; }
        public DbSet<Master_State> Master_State { get; set; }
        public DbSet<Master_City> Master_City { get; set; }
        public DbSet<Master_Roles> Master_Roles { get; set; }
        public DbSet<Master_Menu> Master_Menu { get; set; }
        public DbSet<Master_Designation> Master_Designation { get; set; }
        public DbSet<Master_Permission> Master_Permission { get; set; }
        public DbSet<Master_Industry> Master_Industry { get; set; }
        public DbSet<Master_Path> Master_Path { get; set; }
        public DbSet<Master_PathPermission> Master_PathPermission { get; set; }
        public DbSet<Tenant> Tenant { get; set; }
        public DbSet<Master_Employee> Master_Employee { get; set; }
        public DbSet<TenantDomain> TenantDomain { get; set; }
        public DbSet<Tenant_Domain> Tenant_Domain { get; set; }
        public DbSet<Api_Category> Api_Category { get; set; }
        public DbSet<Auth_PassResetToken> Auth_PassResetToken { get; set; }
        public DbSet<Menu_RolePermission> Menu_RolePermission { get; set; }
        public DbSet<Api_Components> Api_Components { get; set; }
        public DbSet<Api_Provider> Api_Provider { get; set; }
        public DbSet<Api_ProviderMapping> Api_ProviderMapping { get; set; }
        public DbSet<Api_ProviderComponentMapping> Api_ProviderComponentMapping { get; set; }
        public DbSet<Clients> Clients { get; set; }
        public DbSet<RegistrationApiLog> RegistrationApiLog { get; set; }
        public DbSet<Master_Customer> Master_Customer { get; set; }
        public DbSet<CustomerConsent> CustomerConsent { get; set; }
        public DbSet<CustomerVideoKYC> CustomerVideoKYC { get; set; }
        public DbSet<Master_Policy> Master_Policy { get; set; }
        public DbSet<Master_Career> Master_Career { get; set; }
        public DbSet<Master_DosDonts> Master_DosDonts { get; set; }
        public DbSet<Master_FAQCategory> Master_FAQCategory { get; set; }
        public DbSet<Master_Api> Master_Api { get; set; }
        public DbSet<Service_Category> Service_Category { get; set; }
        public DbSet<Certifications> certifications { get; set; }
        public DbSet<Knowledge_Hub_Category> Knowledge_Hub_Category { get; set; }
        public DbSet<Knowledge_Hub> Knowledge_Hub { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<News_Category> News_Category { get; set; }
        public DbSet<OurTeam> OurTeam { get; set; }
        public DbSet<WL_MasterSocialMedia> WL_MasterSocialMedia { get; set; }
        public DbSet<WL_MasterCompanyBasicData> WL_MasterCompanyBasicData { get; set; }
        public DbSet<WL_Clients> WL_Clients { get; set; }
        public DbSet<Master_FinancialYear> Master_FinancialYear { get; set; }
        public DbSet<ActivityLogs> ActivityLogs { get; set; }
        public DbSet<WL_MasterCMS> WL_MasterCMS { get; set; }
        public DbSet<WL_MasterEmailTemplate> WL_MasterEmailTemplate { get; set; }
        public DbSet<Support_TicketCategory> Support_TicketCategory { get; set; }
        public DbSet<Support_TicketLine> Support_TicketLine { get; set; }
        public DbSet<Support_TicketLineAttachment> Support_TicketLineAttachment { get; set; }
        public DbSet<WL_MasterTestimonial> WL_MasterTestimonial { get; set; }
        public DbSet<Support_TicketHeader> Support_TicketHeader { get; set; }
        public DbSet<Password_Policy> Password_Policy { get; set; }
        public DbSet<WL_MasterGender> WL_MasterGender { get; set; }
        public DbSet<WL_MasterHonorific> WL_MasterHonorific { get; set; }
        public DbSet<WL_MasterDocument> WL_MasterDocument { get; set; }
        public DbSet<WL_MasterRoles> WL_MasterRoles { get; set; }
        public DbSet<WL_MasterDepartment> WL_MasterDepartment { get; set; }
        public DbSet<WL_MasterDesignation> WL_MasterDesignation { get; set; }
        public DbSet<WL_MasterFinancialYear> WL_MasterFinancialYear { get; set; }
        public DbSet<WL_MasterNomenClature> WL_MasterNomenClature { get; set; }
        public DbSet<WL_MasterPermissionGroup> WL_MasterPermissionGroup { get; set; }
        public DbSet<WL_MasterEmployee> WL_MasterEmployee { get; set; }
        public DbSet<WL_AdminAuthLogs> WL_AdminAuthLogs { get; set; }

        public DbSet<Credential_Whatsapp> Credential_Whatsapp { get; set; }
        public DbSet<Credential_SMS_Gateway> Credential_SMS_Gateway { get; set; }
        public DbSet<WL_SMSCredential> WL_SMSCredential { get; set; }
        public DbSet<WL_WhatsappCredential> WL_WhatsappCredential { get; set; }
        public DbSet<Api_Endpoint> Api_Endpoint { get; set; }
        public DbSet<Master_FAQSubCategory> Master_FAQSubCategory { get; set; }
        public DbSet<WL_ActivityLogs> WL_ActivityLogs { get; set; }
        public DbSet<VerificationRequestLog> VerificationRequestLogs { get; set; }
        
        public DbSet<Pinned_Services> Pinned_Services { get; set; }
        public DbSet<Master_Pricing> Master_Pricing { get; set; }
        public DbSet<ApiXCodeMapper> ApixCodeMapper { get; set; }

        public DbSet<SavedNews> SavedNews { get; set; } = null!;
        public DbSet<PaymentGatewayCredential> PaymentGatewayCredential { get; set; } = null!;

        public DbSet<Website_MasterFAQ> Website_MasterFAQ { get; set; }
        public DbSet<Website_FAQCategory> Website_FAQCategory { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply Fluent Configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            ApplyTenantFilter(modelBuilder);

            modelBuilder.Entity<ApiQueryResult>()
        .HasNoKey();

            modelBuilder.Entity<ApiSectionResultDto>()
                .HasNoKey();

            modelBuilder.Entity<CategoryApiQueryResult>()
               .HasNoKey();
        }
    }
}