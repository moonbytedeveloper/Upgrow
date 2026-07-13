using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.DTO.Notification;
using Upgrow.Domain.Enums;

namespace Upgrow.Application.Interfaces.Notification
{
    public interface IUserNotificationRepository
    {
        Task<UserNotificationProfileDto?>
            GetProfileAsync(
                string userId);
    }
}
