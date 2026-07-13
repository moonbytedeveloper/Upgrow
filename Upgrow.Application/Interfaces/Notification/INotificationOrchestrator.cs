using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.DTO.Notification;

namespace Upgrow.Application.Interfaces.Notification
{
    public interface INotificationOrchestrator
    {
        Task SendAsync(
            NotificationRequestDto request,
            CancellationToken cancellationToken = default);
    }
}
