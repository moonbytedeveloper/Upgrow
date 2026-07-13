using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.Helper
{
    public class EncryptionHelper
    {
        public static string Encrypt(string plainText)
        {
            try
            {
                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                return Convert.ToBase64String(plainBytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public static string Decrypt(string base64EncodedText)
        {
            try
            {
                byte[] decodedBytes = Convert.FromBase64String(base64EncodedText);
                string decryptedText = Encoding.UTF8.GetString(decodedBytes);

                return decryptedText;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
    }
}