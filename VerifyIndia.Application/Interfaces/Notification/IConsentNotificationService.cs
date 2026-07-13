using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.Interfaces.Notification
{
    public interface IConsentNotificationService
    {
        Task SendConsentLinkAsync(
            string customerUuid,
            string consentToken);
    }
}
