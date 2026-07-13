using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities;

namespace Upgrow.Application.Interfaces.Notification;

public interface INotificationRepository
{
    Task<Master_EmailTemplate?>
        GetEmailTemplateAsync(
            string templateName);

    Task<Master_EmailCredential?>
        GetEmailCredentialAsync(
            string credentialUuid);

    Task<MasterSMSTemplate?>
        GetSmsTemplateAsync(
            string eventCode);

    Task<MasterSMSCredential?>
        GetSmsCredentialAsync(
            string smsCredentialUuid);
}
