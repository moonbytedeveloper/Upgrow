using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Notification;
using Upgrow.Application.DTO.Notification;

namespace Upgrow.Application.IServices.Notification
{
    public interface ICustomerNotificationPreferenceService
    {
        Task<CustomerNotificationPreferenceDto?> GetByUuidAsync(string customerUuid);
        Task SaveAsync(CustomerNotificationPreferenceCommand command, string userUuid, string ip, bool saveChanges = true);
        Task<CustomerNotificationPreferenceDto?> GetByCustomerUuidAsync(string customerUuid);
    }
}
