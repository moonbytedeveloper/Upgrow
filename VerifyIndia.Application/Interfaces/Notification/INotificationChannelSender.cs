using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.Notification;
using VerifyIndia.Domain.Enums;

namespace VerifyIndia.Application.Interfaces.Notification
{
    public interface INotificationChannelSender
    {
        NotificationChannel Channel { get; }

        Task SendAsync(
            NotificationMessageDto message,
            CancellationToken cancellationToken = default);
    }
}
