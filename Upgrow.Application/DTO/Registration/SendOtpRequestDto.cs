using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Enums;

namespace Upgrow.Application.DTO.Registration
{
    public sealed class SendOtpRequestDto
    {
        [Required]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Please enter a valid mobile number.")]
        public string MobileNumber { get; set; } = string.Empty;
        public string? ReferralCode { get; set; } = string.Empty;

        public NotificationChannel Channel
        {
            get;
            set;
        } = NotificationChannel.Sms;
    }
}
