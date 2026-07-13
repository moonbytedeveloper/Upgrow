using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Upgrow.Application.DTO.Notification;
using Upgrow.Application.Interfaces.Notification;
using Upgrow.Application.IServices;
using Upgrow.Domain.Enums;

namespace Upgrow.Infrastructure.Notifications.Channels;

public sealed class EmailSender
    : INotificationChannelSender
{
    private readonly INotificationRepository
        _notificationRepository;

    private readonly ITemplateRenderer
        _templateRenderer;

    private readonly IEncryptionService
        _encryptionService;

    public NotificationChannel Channel
        => NotificationChannel.Email;

    public EmailSender(
        IEncryptionService encryptionService,
        INotificationRepository notificationRepository,
        ITemplateRenderer templateRenderer)
    {
        _encryptionService = 
            encryptionService;

        _notificationRepository =
            notificationRepository;

        _templateRenderer =
            templateRenderer;
    }

    public async Task SendAsync(
        NotificationMessageDto message,
        CancellationToken cancellationToken = default)
    {
        var template =
            await _notificationRepository
                .GetEmailTemplateAsync(message.EventCode);

        if (template is null)
        {
            throw new InvalidOperationException(
                $"Email template not found for '{message.EventCode}'.");
        }

        var credential =
            await _notificationRepository
                .GetEmailCredentialAsync(template.EmailCredentialUUID!);

        if (credential is null)
        {
            throw new InvalidOperationException(
                $"Email credential not found for template '{message.EventCode}'.");
        }

        var subject =
            _templateRenderer.Render(
                template.EmailSubject ?? string.Empty,
                message.Variables);

        var body =
            _templateRenderer.Render(
                template.Description ?? string.Empty,
                message.Variables);

        var email = new MimeMessage();

        email.From.Add(
            MailboxAddress.Parse(
                credential.EmailAddress));

        email.To.Add(
            MailboxAddress.Parse(
                message.Recipient));

        email.Subject = subject;

        email.Body =
            new BodyBuilder
            {
                HtmlBody = body
            }
            .ToMessageBody();

        using var smtp = new SmtpClient();

        await smtp.ConnectAsync(
            credential.HostServiceProvider,
            Convert.ToInt32(credential.Port),
            SecureSocketOptions.StartTls,
            cancellationToken);

        await smtp.AuthenticateAsync(
            credential.EmailAddress,
            _encryptionService.Decrypt(credential.Password),
            cancellationToken);

        await smtp.SendAsync(
            email,
            cancellationToken);

        await smtp.DisconnectAsync(
            true,
            cancellationToken);
    }
}