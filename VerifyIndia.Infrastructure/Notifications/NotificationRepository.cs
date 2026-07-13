using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Interfaces.Notification;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Infrastructure.Notifications;

public sealed class NotificationRepository
    : INotificationRepository
{
    private readonly AppDbContext _context;

    public NotificationRepository(
        AppDbContext context)
    {
        _context = context;
    }

    public async Task<Master_EmailTemplate?> GetEmailTemplateAsync(
        string eventCode)
    {
        if (string.IsNullOrWhiteSpace(
                eventCode))
        {
            throw new ArgumentException(
                "Event code is required.",
                nameof(eventCode));
        }

        return await _context.Master_EmailTemplate
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x =>
                    x.TemplateCode == eventCode &&
                    x.IsActive);
    }

    public async Task<Master_EmailCredential?> GetEmailCredentialAsync(
        string emailCredentialUuid)
    {
        if (string.IsNullOrWhiteSpace(
                emailCredentialUuid))
        {
            throw new ArgumentException(
                "Email credential UUID is required.",
                nameof(emailCredentialUuid));
        }

        return await _context.Master_EmailCredential
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x =>
                    x.UUID == emailCredentialUuid &&
                    x.IsActive);
    }

    public async Task<MasterSMSTemplate?> GetSmsTemplateAsync(
        string eventCode)
    {
        if (string.IsNullOrWhiteSpace(
                eventCode))
        {
            throw new ArgumentException(
                "Event code is required.",
                nameof(eventCode));
        }

        return await _context
            .MasterSMSTemplate
            .AsNoTracking()
            .Where(x =>
                x.EventCode == eventCode
                &&
                x.IsActive)
            .OrderByDescending(x => x.Id)
            .FirstOrDefaultAsync();
    }

    public async Task<MasterSMSCredential?> GetSmsCredentialAsync(
        string smsCredentialUuid)
    {
        if (string.IsNullOrWhiteSpace(
                smsCredentialUuid))
        {
            throw new ArgumentException(
                "SMS credential UUID is required.",
                nameof(smsCredentialUuid));
        }

        return await _context
            .MasterSMSCredential
            .AsNoTracking()
            .Where(x =>
                x.UUID == smsCredentialUuid
                &&
                x.IsActive)
            .OrderByDescending(x => x.Id)
            .FirstOrDefaultAsync();
    }
}
