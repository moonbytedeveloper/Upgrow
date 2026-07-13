using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.Notification;
using VerifyIndia.Application.Interfaces.Notification;

namespace VerifyIndia.Infrastructure.Notifications;

public sealed class UserNotificationRepository
    : IUserNotificationRepository
{
    private readonly AppDbContext _context;

    public UserNotificationRepository(
        AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserNotificationProfileDto?> GetProfileAsync(string userId)
    {
        // Check Customer
        var customer = await _context.Master_Customer
            .AsNoTracking()
            .Where(x => x.UUID == userId && x.IsActive)
            .Select(x => new UserNotificationProfileDto
            {
                UserId = x.UUID,
                Email = x.Email,
                Mobile = x.Mobile,
                EmailEnabled = true,
                SmsEnabled = true,
                WhatsAppEnabled = true,
                PushEnabled = true
            })
            .FirstOrDefaultAsync();

        if (customer != null)
            return customer;

        // Check Agent
        //var agent = await _context.Master_Agent
        //    .AsNoTracking()
        //    .Where(x => x.UUID == userId && x.IsActive)
        //    .Select(x => new UserNotificationProfileDto
        //    {
        //        UserId = x.UUID,
        //        Email = x.Email,
        //        Mobile = x.Mobile,
        //        EmailEnabled = true,
        //        SmsEnabled = true,
        //        WhatsAppEnabled = true,
        //        PushEnabled = true
        //    })
        //    .FirstOrDefaultAsync();

        //if (agent != null)
        //    return agent;

        // Check Employee
        var employee = await _context.Master_Employee
            .AsNoTracking()
            .Where(x => x.UUID == userId && x.IsActive)
            .Select(x => new UserNotificationProfileDto
            {
                UserId = x.UUID,
                Email = x.EmailId,
                Mobile = x.MobileNumber,
                EmailEnabled = true,
                SmsEnabled = true,
                WhatsAppEnabled = true,
                PushEnabled = true
            })
            .FirstOrDefaultAsync();

        if (employee != null)
            return employee;

        return null;
    }
}
