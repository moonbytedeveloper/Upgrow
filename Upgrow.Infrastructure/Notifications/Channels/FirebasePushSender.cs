using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Constant;
using Upgrow.Application.DTO.Notification;
using Upgrow.Application.Interfaces.Notification;
using Upgrow.Domain.Enums;

namespace Upgrow.Infrastructure.Notifications.Channels;

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
