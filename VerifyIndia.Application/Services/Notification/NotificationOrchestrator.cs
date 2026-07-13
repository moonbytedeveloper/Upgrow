using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Constant;
using Upgrow.Application.DTO.Notification;
using Upgrow.Application.Interfaces.Notification;
using Upgrow.Application.IServices.Auth;
using Upgrow.Domain.Enums;

namespace Upgrow.Application.Services.Notification;

public sealed class NotificationOrchestrator
    : INotificationOrchestrator
{
    private readonly IEnumerable<INotificationChannelSender>
        _senders;

    private readonly IUserNotificationRepository
        _userRepository;

    public NotificationOrchestrator(
        IEnumerable<INotificationChannelSender> senders,
        IUserNotificationRepository userRepository)
    {
        _senders = senders;
        _userRepository = userRepository;
    }

    public async Task SendAsync(
        NotificationRequestDto request,
        CancellationToken cancellationToken = default)
    {
        if (!NotificationRouting.Routes.TryGetValue(
                request.EventCode,
                out var routedChannels))
        {
            throw new InvalidOperationException(
                $"Notification route not configured for '{request.EventCode}'.");
        }

        var profile =
            await _userRepository.GetProfileAsync(
                request.UserId);

        if (profile is null)
        {
            return;
        }

        NotificationChannel[] channelsToSend;
        var bypassUserPreferences = false;

        if (request.Channels is { Count: > 0 })
        {
            var invalidChannels =
                request.Channels
                    .Except(routedChannels)
                    .ToArray();

            if (invalidChannels.Length > 0)
            {
                throw new InvalidOperationException(
                    $"Channel(s) '{string.Join(", ", invalidChannels)}' are not configured for event '{request.EventCode}'.");
            }

            channelsToSend =
                request.Channels.ToArray();

            bypassUserPreferences = true;
        }
        else
        {
            channelsToSend = routedChannels;
        }

        foreach (var channel in channelsToSend)
        {
            if (!bypassUserPreferences)
            {
                var enabled = channel switch
                {
                    NotificationChannel.Email =>
                        profile.EmailEnabled,

                    NotificationChannel.Sms =>
                        profile.SmsEnabled,

                    NotificationChannel.WhatsApp =>
                        profile.WhatsAppEnabled,

                    NotificationChannel.Push =>
                        profile.PushEnabled,

                    _ => false
                };

                if (!enabled)
                {
                    continue;
                }
            }

            var recipient = channel switch
            {
                NotificationChannel.Email =>
                    !string.IsNullOrWhiteSpace(request.Email)
                        ? request.Email
                        : profile.Email,

                NotificationChannel.Sms =>
                    !string.IsNullOrWhiteSpace(request.Mobile)
                        ? request.Mobile
                        : profile.Mobile,

                NotificationChannel.WhatsApp =>
                    !string.IsNullOrWhiteSpace(request.Mobile)
                        ? request.Mobile
                        : profile.Mobile,

                NotificationChannel.Push =>
                    !string.IsNullOrWhiteSpace(request.DeviceToken)
                        ? request.DeviceToken
                        : profile.UserId,

                _ => null
            };

            if (string.IsNullOrWhiteSpace(recipient))
            {
                continue;
            }

            var message =
                new NotificationMessageDto
                {
                    Channel = channel,
                    Recipient = recipient,
                    EventCode = request.EventCode,
                    Variables = request.Variables
                };

            if (channel == NotificationChannel.WhatsApp)
            {
                message.TemplateName =
                    WhatsAppTemplates.Templates[request.EventCode];
            }

            var sender =
                _senders.Single(
                    x => x.Channel == channel);

            await sender.SendAsync(
                message,
                cancellationToken);
        }
    }
}