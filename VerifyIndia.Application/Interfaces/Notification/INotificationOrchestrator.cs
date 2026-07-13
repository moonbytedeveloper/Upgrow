using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.Notification;

namespace VerifyIndia.Application.Interfaces.Notification
{
    public interface INotificationOrchestrator
    {
        Task SendAsync(
            NotificationRequestDto request,
            CancellationToken cancellationToken = default);
    }
}
