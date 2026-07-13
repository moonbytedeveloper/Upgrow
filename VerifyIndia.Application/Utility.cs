using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VerifyIndia.Application.Common;

namespace VerifyIndia.Application
{
    public class Utils
    {
        public static string GenerateOtp(int length = 6)
        {
            if (length <= 0)
            {
                throw new ArgumentException(
                    "OTP length must be greater than zero.",
                    nameof(length));
            }

            const string digits =
                "0123456789";

            var otp =
                new char[length];

            for (var i = 0; i < length; i++)
            {
                otp[i] =
                    digits[
                        RandomNumberGenerator.GetInt32(
                            digits.Length)];
            }

            return new string(
                otp);
        }

        public static string? MaskAadhaar(
            string? aadhaarNumber)
        {
            if (string.IsNullOrWhiteSpace(
                    aadhaarNumber))
            {
                return null;
            }

            if (aadhaarNumber.Length < 4)
            {
                return aadhaarNumber;
            }

            return
                $"XXXX XXXX {aadhaarNumber[^4..]}";
        }
        public static string GenerateToken(int length = 16)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            var bytes = RandomNumberGenerator.GetBytes(length);
            var result = new char[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = chars[bytes[i] % chars.Length];
            }

            return new string(result);
        }

        public static string GenerateHash(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Value cannot be null or empty.",
                    nameof(value));
            }

            using var sha256 = SHA256.Create();

            var bytes =
                Encoding.UTF8.GetBytes(value);

            var hashBytes =
                sha256.ComputeHash(bytes);

            return Convert.ToHexString(hashBytes);
        }

        public static DateTime GetCurrentUtcTime()
        {
            return DateTime.UtcNow;
        }

        public static DateTime GetCurrentIndianTime()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(
                DateTime.UtcNow,
                TimeZoneInfo.FindSystemTimeZoneById(
                    "India Standard Time"));
        }

        public static string GetCurrentIndianDate()
        {
            return GetCurrentIndianTime()
                .ToString("dd-MM-yyyy");
        }

        public static string GetCurrentIndianTimeString()
        {
            return GetCurrentIndianTime()
                .ToString("hh:mm:ss tt");
        }

        public static string GetUUID()
        {
            string uuid = Guid.NewGuid().ToString("N");
            string numb = uuid.Substring(0, 8) + "-" + uuid.Substring(8, 4)
                + "-" + uuid.Substring(12, 4) + "-" + uuid.Substring(16, 4)
                + "-" + uuid.Substring(20, 8);
            return numb;
        }

        public static decimal GetRecordNo()
        {
            DateTime currentTime = GetCurrentUtcTime();

            decimal Dat = Convert.ToDecimal(currentTime.ToString("dd/MM/yyyy HH:mm:ss").Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", ""));
            return Dat;
        }

        public static string GetLocalIPAddress()
        {

            var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "";
        }

        public static string GetUserIp(HttpContext context)
        {
            try
            {
                var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();

                if (!string.IsNullOrEmpty(ip))
                    return ip.Split(',').First().Trim();

                return context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            }
            catch
            {
                return "Unknown";
            }
        }

         



    }
}
