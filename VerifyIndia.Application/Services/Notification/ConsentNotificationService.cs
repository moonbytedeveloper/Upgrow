using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using VerifyIndia.Application.Constant;
using VerifyIndia.Application.DTO.Notification;
using VerifyIndia.Application.Interfaces.Notification;

namespace VerifyIndia.Application.Services.Notification
{
    public class ConsentNotificationService
        : IConsentNotificationService
    {
        private readonly INotificationOrchestrator _notificationOrchestrator;

        public ConsentNotificationService(
            INotificationOrchestrator notificationOrchestrator)
        {
            _notificationOrchestrator = notificationOrchestrator;
        }

        public async Task SendConsentLinkAsync(
            string customerUuid,
            string consentToken)
        {
            await _notificationOrchestrator
                    .SendAsync(
                        new NotificationRequestDto
                        {
                            EventCode =
                                NotificationEvents.ConsentLink,

                            UserId =
                                customerUuid,

                            Variables =
                                new()
                                {
                                    ["ConsentToken"] = consentToken,
                                }
                        });
        }
    }
}
