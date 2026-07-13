using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.Commands.Notification;
using VerifyIndia.Application.DTO.CustomerPanel;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.DTO.Notification;
using VerifyIndia.Application.IServices.Master;

namespace VerifyIndia.Application.IServices.Notification
{
    public interface INotificationSoundService : IMasterService<NotificationSoundDto, NotificationSoundCommand>
    {
        Task<List<NotificationSoundDto>> GetAllActiveAsync();
    }
}
