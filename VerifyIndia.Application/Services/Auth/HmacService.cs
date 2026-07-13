using System.Security.Cryptography;
using System.Text;
using Upgrow.Application.IServices.Auth;

namespace Upgrow.Application.Services.Auth
{
    public class HmacService : IHmacService
    {
        public string GenerateSignature(long unixTimestampSeconds, string body, string clientSecret)
        {
            var payload = $"{unixTimestampSeconds}:{body}";
            var keyBytes = Encoding.UTF8.GetBytes(clientSecret);
            var payloadBytes = Encoding.UTF8.GetBytes(payload);

            using var hmac = new HMACSHA256(keyBytes);
            var hash = hmac.ComputeHash(payloadBytes);
            return Convert.ToHexString(hash).ToLowerInvariant();
        }

        public bool ValidateSignature(string providedSignature, long unixTimestampSeconds, string body, string clientSecret)
        {
            if (string.IsNullOrWhiteSpace(providedSignature))
                return false;        
            
            var expected = GenerateSignature(unixTimestampSeconds, body, clientSecret);
            try
            {               
                var providedBytes = Convert.FromHexString(providedSignature.Trim());
                var expectedBytes = Convert.FromHexString(expected);
                
                return providedBytes.Length == expectedBytes.Length &&
                       CryptographicOperations.FixedTimeEquals(providedBytes, expectedBytes);
            }
            catch
            {
                return false;
            }
        }

        public string GenerateHash(string input)
        {
            using var sha256 = SHA256.Create();
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = sha256.ComputeHash(inputBytes);
            return Convert.ToHexString(hashBytes).ToLowerInvariant();
        }
    }
}
