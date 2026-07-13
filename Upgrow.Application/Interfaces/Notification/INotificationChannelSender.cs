using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.DTO.Notification;
using Upgrow.Domain.Enums;

namespace Upgrow.Application.Interfaces.Notification
{
    public interface INotificationChannelSender
    {
        NotificationChannel Channel { get; }

        Task SendAsync(
            NotificationMessageDto message,
            CancellationToken cancellationToken = default);
    }
}
