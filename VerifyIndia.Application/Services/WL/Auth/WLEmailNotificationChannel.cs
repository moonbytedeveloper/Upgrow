using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.IServices.Auth;
using VerifyIndia.Application.Services.Auth;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;
using VerifyIndia.Domain.IRepositories.Master;
using VerifyIndia.Domain.IRepositories.WL;

namespace VerifyIndia.Application.Services.WL.Auth
{
    public class WLEmailNotificationChannel : INotificationChannel
    {
        private readonly ILogger<WLEmailNotificationChannel> _logger;
        private readonly IMapper _mapper; // 
        private readonly IWLMasterEmailTemplateRepository _templateRepository;
        private readonly IMasterRepository<WL_MasterEmailCredential> _credentialRepository;

        public WLEmailNotificationChannel(
            IWLMasterEmailTemplateRepository templateRepository,
            IMasterRepository<WL_MasterEmailCredential> credentialRepository, IMapper mapper,
            ILogger<WLEmailNotificationChannel> logger)
        {
            _logger = logger;
            _templateRepository = templateRepository;
            _credentialRepository = credentialRepository;
            _mapper = mapper;
        }

        public string ChannelName => "Email";

        public bool CanHandle(NotificationContext context)
        {
            return !string.IsNullOrWhiteSpace(context.RecipientEmail)
                && context.IsWhiteLabel == true
                   && context.EmailTemplateId.HasValue;
        }


        public async Task<NotificationChannelResult> SendAsync(NotificationContext context)
        {
            var result = new NotificationChannelResult
            {
                ChannelName = ChannelName,
                SentAt = DateTime.UtcNow
            };

            try
            {
                if (!CanHandle(context))
                {
                    result.IsSuccess = false;
                    result.Message = "Invalid email notification context";
                    return result;
                }

                var emailConfig =
                    await GetEmailTemplateAndCredentialAsync(
                        context.EmailTemplateId!.Value);

                // Validate host and port (fail fast and log enough info to debug)
                if (string.IsNullOrWhiteSpace(emailConfig.Host))
                    throw new InvalidOperationException("SMTP host is empty for the configured credential.");

                if (emailConfig.Port <= 0 || emailConfig.Port > 65535)
                    throw new InvalidOperationException($"SMTP port is invalid: {emailConfig.Port}");

                _logger.LogInformation("Preparing to send email using template {TemplateId} and SMTP host {Host}:{Port}",
                    context.EmailTemplateId, emailConfig.Host, emailConfig.Port);

                string subject = ReplacePlaceholders(
                    emailConfig.Subject, context.Placeholders);

                string body = ReplacePlaceholders(
                    emailConfig.Body, context.Placeholders);

                using var mail = new MailMessage
                {
                    From = new MailAddress(
                        emailConfig.FromEmail,
                        context.SenderName ?? ""),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mail.To.Add(context.RecipientEmail);

                if (context.CcEmails?.Any() == true)
                {
                    foreach (var cc in context.CcEmails.Where(e => !string.IsNullOrWhiteSpace(e)))
                        mail.CC.Add(cc);
                }

                using var smtp = new SmtpClient(emailConfig.Host, emailConfig.Port)
                {
                    Credentials = new NetworkCredential(
                        emailConfig.FromEmail,
                        emailConfig.Password),
                    EnableSsl = true,
                    Timeout = 180000
                };

                // Use the native async API instead of Task.Run
                await smtp.SendMailAsync(mail);


                result.IsSuccess = true;
                result.Message = "Email sent successfully";
                result.Metadata["RecipientEmail"] = context.RecipientEmail;
                result.Metadata["TemplateId"] = context.EmailTemplateId;

                _logger.LogInformation(
                    "Email sent to {Email} using template {TemplateId}",
                    context.RecipientEmail,
                    context.EmailTemplateId);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
                result.Error = ex;

                _logger.LogError(
                    ex,
                    "Failed to send email to {Email}",
                    context.RecipientEmail);
            }

            return result;
        }

        private async Task<EmailConfig> GetEmailTemplateAndCredentialAsync(decimal templateId)
        {
            var template = await _templateRepository.GetByIdAsync(templateId);

            if (template == null)
                throw new Exception($"Email template with ID {templateId} not found");


            if (string.IsNullOrEmpty(template.EmailCredentialUUID))
                throw new Exception("EmailCredential_UUID missing in template");

            var entityCredential = await _credentialRepository.GetByUuidAsync(template.EmailCredentialUUID);

            if (entityCredential == null)
                throw new Exception($"Email credential with UUID {template.EmailCredentialUUID} not found");

            // ✅ Map entity to DTO using AutoMapper
            var credential = _mapper.Map<WLMasterEmailCredentialDto>(entityCredential);
            int port = 587;
            if (!string.IsNullOrWhiteSpace(credential.Port))
            {
                if (int.TryParse(credential.Port, out var parsedPort) && parsedPort > 0)
                {
                    port = parsedPort;
                }
            }
            return new EmailConfig
            {
                Subject = template.EmailSubject ?? "",
                Body = template.Description ?? "",
                FromEmail = credential.EmailAddress,
                Password = credential.Password,
                Host = credential.HostServiceProvider,
                Port = port
            };
        }

        private string ReplacePlaceholders(
            string template,
            Dictionary<string, string> placeholders)
        {
            if (string.IsNullOrEmpty(template) || placeholders == null)
                return template;

            foreach (var item in placeholders)
                template = template.Replace(item.Key, item.Value);

            return template;
        }

        private sealed class EmailConfig
        {
            public string Subject { get; set; } = "";
            public string Body { get; set; } = "";
            public string FromEmail { get; set; } = "";
            public string Password { get; set; } = "";
            public string Host { get; set; } = "";
            public int Port { get; set; }
        }
    }
}
