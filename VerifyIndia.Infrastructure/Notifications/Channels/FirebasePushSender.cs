using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Constant;
using VerifyIndia.Application.DTO.Notification;
using VerifyIndia.Application.Interfaces.Notification;
using VerifyIndia.Domain.Enums;

namespace VerifyIndia.Infrastructure.Notifications.Channels;

public sealed class FirebasePushSender
    : INotificationChannelSender
{
    public NotificationChannel Channel
        => NotificationChannel.Push;

    public async Task SendAsync(
        NotificationMessageDto message,
        CancellationToken cancellationToken)
    {
        /*var title =
            PushMessages.Titles[
                message.EventCode];

        var body =
            PushMessages.Bodies[
                message.EventCode];*/

        // Send Firebase notification
    }
}
