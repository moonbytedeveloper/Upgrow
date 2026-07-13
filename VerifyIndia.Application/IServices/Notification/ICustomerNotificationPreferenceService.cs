using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Notification;
using VerifyIndia.Application.DTO.Notification;

namespace VerifyIndia.Application.IServices.Notification
{
    public interface ICustomerNotificationPreferenceService
    {
        Task<CustomerNotificationPreferenceDto?> GetByUuidAsync(string customerUuid);
        Task SaveAsync(CustomerNotificationPreferenceCommand command, string userUuid, string ip, bool saveChanges = true);
        Task<CustomerNotificationPreferenceDto?> GetByCustomerUuidAsync(string customerUuid);
    }
}
