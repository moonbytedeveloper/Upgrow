using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities.WL.Master;

namespace VerifyIndia.Application.Utilities
{
    public static class WLActivityLogHashUtility
    {
        /// <summary>
        /// Compute SHA256 hash of the payload
        /// Returns byte array (not string like ActivityLogHashUtility)
        /// </summary>
        public static byte[] ComputeCurrentHash(string payload)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(payload));
            return bytes;
        }

        /// <summary>
        /// Compute RSA digital signature of the current hash
        /// </summary>
        public static byte[] ComputeDigitalSignature(byte[] currentHash, string privateKeyFilePath)
        {
            if (string.IsNullOrWhiteSpace(privateKeyFilePath))
            {
                throw new InvalidOperationException("RSA private key file path not provided.");
            }

            string privateKeyPem;
            try
            {
                privateKeyPem = File.ReadAllText(privateKeyFilePath);
            }
            catch (FileNotFoundException)
            {
                throw new InvalidOperationException($"RSA private key file not found at path: {privateKeyFilePath}. Ensure the .pem file exists in wwwroot.");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error reading RSA private key file: {ex.Message}", ex);
            }

            if (string.IsNullOrWhiteSpace(privateKeyPem))
            {
                throw new InvalidOperationException("RSA private key file is empty or invalid.");
            }

            try
            {
                using var rsa = RSA.Create();
                rsa.ImportFromPem(privateKeyPem);
                var signatureBytes = rsa.SignHash(currentHash, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                return signatureBytes;
            }
            catch (CryptographicException ex)
            {
                throw new InvalidOperationException("Invalid RSA private key format in .pem file. Ensure it's a valid PKCS#8 PEM-encoded private key.", ex);
            }
        }       
    }
}
