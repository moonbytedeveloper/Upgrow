using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Constant;
using Upgrow.Application.DTO.Notification;
using Upgrow.Application.Interfaces.Notification;
using Upgrow.Domain.Enums;

namespace Upgrow.Infrastructure.Notifications.Channels;

public sealed class WhatsappSender
    : INotificationChannelSender
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public NotificationChannel Channel
        => NotificationChannel.WhatsApp;

    public WhatsappSender(
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task SendAsync(
        NotificationMessageDto message,
        CancellationToken cancellationToken = default)
    {
        if (!WhatsAppTemplates.Templates.TryGetValue(
                message.EventCode,
                out var templateName))
        {
            throw new InvalidOperationException(
                $"WhatsApp template not configured for event '{message.EventCode}'.");
        }

        var phoneNumberId =
            _configuration["WhatsApp:PhoneNumberId"];

        var accessToken =
            _configuration["WhatsApp:AccessToken"];

        if (string.IsNullOrWhiteSpace(phoneNumberId))
        {
            throw new InvalidOperationException(
                "WhatsApp PhoneNumberId not configured.");
        }

        if (string.IsNullOrWhiteSpace(accessToken))
        {
            throw new InvalidOperationException(
                "WhatsApp AccessToken not configured.");
        }

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                accessToken);

        var payload = message.EventCode switch
        {
            NotificationEvents.LoginOtp
                => BuildLoginOtpPayload(
                    message,
                    templateName),

            NotificationEvents.ConsentLink
                => BuildConsentPayload(
                    message,
                    templateName),

            _ => throw new NotSupportedException(
                $"WhatsApp template '{templateName}' is not supported.")
        };

        var response =
            await _httpClient.PostAsJsonAsync(
                $"https://graph.facebook.com/v25.0/{phoneNumberId}/messages",
                payload,
                cancellationToken);

        var responseContent =
            await response.Content.ReadAsStringAsync(
                cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(
                $"WhatsApp send failed. Status: {response.StatusCode}. Response: {responseContent}");
        }
    }

    private static object BuildLoginOtpPayload(
        NotificationMessageDto message,
        string templateName)
    {
        if (!message.Variables.TryGetValue(
                "Otp",
                out var otpValue))
        {
            throw new InvalidOperationException(
                "Otp variable is required.");
        }

        var otp = otpValue?.ToString();

        return new
        {
            messaging_product = "whatsapp",
            to = message.Recipient,
            type = "template",

            template = new
            {
                name = templateName,

                language = new
                {
                    code = "en"
                },

                components = new object[]
                {
                    new
                    {
                        type = "body",

                        parameters = new object[]
                        {
                            new
                            {
                                type = "text",
                                text = otp
                            }
                        }
                    },

                    new
                    {
                        type = "button",

                        sub_type = "url",

                        index = "0",

                        parameters = new object[]
                        {
                            new
                            {
                                type = "text",
                                text = otp
                            }
                        }
                    }
                }
            }
        };
    }

    private static object BuildConsentPayload(
    NotificationMessageDto message,
    string templateName)
    {
        if (!message.Variables.TryGetValue(
                "ConsentToken",
                out var tokenValue))
        {
            throw new InvalidOperationException(
                "ConsentToken variable is required.");
        }

        var token = tokenValue?.ToString();

        return new
        {
            messaging_product = "whatsapp",
            to = message.Recipient,
            type = "template",

            template = new
            {
                name = templateName,

                language = new
                {
                    code = "en"
                },

                components = new object[]
                {
                new
                {
                    type = "button",

                    sub_type = "url",

                    index = "0",

                    parameters = new object[]
                    {
                        new
                        {
                            type = "text",
                            text = token
                        }
                    }
                }
                }
            }
        };
    }
}