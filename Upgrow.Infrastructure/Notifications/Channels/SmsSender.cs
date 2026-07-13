using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Upgrow.Application.DTO.Notification;
using Upgrow.Application.Interfaces.Notification;
using Upgrow.Application.IServices;
using Upgrow.Domain.Enums;

namespace Upgrow.Infrastructure.Notifications.Channels;

public sealed class SmsSender
        : INotificationChannelSender
{
    private readonly IHttpClientFactory
        _httpClientFactory;

    private readonly INotificationRepository
        _notificationRepository;

    private readonly IEncryptionService
        _encryptionService;

    public NotificationChannel Channel
        => NotificationChannel.Sms;

    public SmsSender(
        IEncryptionService encryptionService,
        IHttpClientFactory httpClientFactory,
        INotificationRepository notificationRepository)
    {
        _encryptionService =
            encryptionService;

        _httpClientFactory =
            httpClientFactory;

        _notificationRepository =
            notificationRepository;
    }

    public async Task SendAsync(
        NotificationMessageDto message,
        CancellationToken cancellationToken = default)
    {
        var template =
            await _notificationRepository
                .GetSmsTemplateAsync(
                    message.EventCode);

        if (template == null)
        {
            throw new InvalidOperationException(
                $"SMS template not found for event '{message.EventCode}'.");
        }

        var credential =
            await _notificationRepository
                .GetSmsCredentialAsync(
                    template.SMSCredentialUUID);

        if (credential == null)
        {
            throw new InvalidOperationException(
                $"SMS credential not found for event '{message.EventCode}'.");
        }

        var recipient =
            new Dictionary<string, object?>
            {
                ["mobiles"] =
                    "91" + message.Recipient
            };

        foreach (var variable in message.Variables)
        {
            recipient[variable.Key] =
                variable.Value;
        }

        var payload =
            new
            {
                template_id =
                    template.ProviderTemplateId,

                recipients =
                    new[]
                    {
                            recipient
                    }
            };

        var json =
            JsonSerializer.Serialize(
                payload);

        using var client =
            _httpClientFactory
                .CreateClient();

        client.DefaultRequestHeaders.Accept
            .Add(
                new MediaTypeWithQualityHeaderValue(
                    "application/json"));

        client.DefaultRequestHeaders.Add(
            "authkey",
            _encryptionService.Decrypt(credential.AuthKey));

        using var content =
            new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

        using var response =
            await client.PostAsync(
                credential.BaseUrl,
                content,
                cancellationToken);

        var responseBody =
            await response.Content
                .ReadAsStringAsync(
                    cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(
                $"SMS sending failed. Status Code: {(int)response.StatusCode}. Response: {responseBody}");
        }

        using var document =
            JsonDocument.Parse(
                responseBody);

        if (document.RootElement.TryGetProperty(
                "type",
                out var typeElement))
        {
            var type =
                typeElement.GetString();

            if (!string.Equals(
                    type,
                    "success",
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    $"SMS sending failed. Response: {responseBody}");
            }
        }
    }
}