using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using VerifyIndia.Application.DTO.Auth;
using VerifyIndia.Application.IServices;
using Microsoft.Extensions.Configuration;

namespace VerifyIndia.Application.Services
{
    public sealed class EncryptionService : IEncryptionService
    {
        private const int KeySize = 32;      // 256-bit
        private const int NonceSize = 12;    // Recommended for GCM
        private const int TagSize = 16;      // 128-bit authentication tag

        private readonly byte[] _key;

        public EncryptionService(IConfiguration configuration)
        {
            var key = configuration["EncryptionSettings:Key"]
                      ?? throw new InvalidOperationException(
                          "Encryption key is not configured.");

            try
            {
                _key = Convert.FromBase64String(key);
            }
            catch (FormatException)
            {
                throw new InvalidOperationException(
                    "Encryption key must be a valid Base64 string.");
            }

            if (_key.Length != KeySize)
            {
                throw new InvalidOperationException(
                    $"Encryption key must be {KeySize} bytes (256-bit).");
            }
        }

        public string Encrypt(string plainText)
        {
            ArgumentNullException.ThrowIfNull(plainText);

            var nonce = RandomNumberGenerator.GetBytes(NonceSize);
            var plainBytes = Encoding.UTF8.GetBytes(plainText);

            var cipherBytes = new byte[plainBytes.Length];
            var tag = new byte[TagSize];

            using var aes = new AesGcm(_key, TagSize);

            aes.Encrypt(
                nonce,
                plainBytes,
                cipherBytes,
                tag);

            var payload = new byte[
                NonceSize +
                TagSize +
                cipherBytes.Length];

            Buffer.BlockCopy(nonce,0,payload,0,NonceSize);

            Buffer.BlockCopy(tag,0,payload,NonceSize,TagSize);

            Buffer.BlockCopy(cipherBytes,0,payload,NonceSize + TagSize,cipherBytes.Length);

            return Convert.ToBase64String(payload);
        }

        public string Decrypt(string cipherText)
        {
            ArgumentNullException.ThrowIfNull(cipherText);

            byte[] payload;

            try
            {
                payload = Convert.FromBase64String(cipherText);
            }
            catch (FormatException)
            {
                throw new CryptographicException(
                    "Invalid encrypted payload format.");
            }

            if (payload.Length < NonceSize + TagSize)
            {
                throw new CryptographicException(
                    "Invalid encrypted payload.");
            }

            var nonce = payload[..NonceSize];
            var tag = payload[NonceSize..(NonceSize + TagSize)];
            var cipherBytes = payload[(NonceSize + TagSize)..];

            var plainBytes = new byte[cipherBytes.Length];

            try
            {
                using var aes = new AesGcm(_key, TagSize);

                aes.Decrypt(
                    nonce,
                    cipherBytes,
                    tag,
                    plainBytes);

                return Encoding.UTF8.GetString(plainBytes);
            }
            catch (CryptographicException)
            {
                throw new CryptographicException(
                    "Decryption failed. The data may have been tampered with or the encryption key is incorrect.");
            }
        }
    }
}
