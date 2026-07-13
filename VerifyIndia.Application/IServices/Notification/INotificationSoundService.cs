using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.Commands.Notification;
using Upgrow.Application.DTO.CustomerPanel;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.DTO.Notification;
using Upgrow.Application.IServices.Master;

namespace Upgrow.Application.IServices.Notification
{
    public interface INotificationSoundService : IMasterService<NotificationSoundDto, NotificationSoundCommand>
    {
        Task<List<NotificationSoundDto>> GetAllActiveAsync();
    }
}
