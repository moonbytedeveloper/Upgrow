using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Upgrow.Application.DTO.Verification.Common.Request;
using Upgrow.Application.Verification.Interfaces;
using static Upgrow.Application.Constants;

namespace Upgrow.Application.Verification
{
    public sealed class VerificationRequestFactory
        : IVerificationRequestFactory
    {
        private readonly Dictionary<
            string,
            Func<string?, object?>> _requestFactories;

        public VerificationRequestFactory()
        {
            _requestFactories =
                new(StringComparer.OrdinalIgnoreCase)
                {
                    [VerificationCodes.Pan] =
                        json => Deserialize<PanVerifyRequest>(json),

                    [VerificationCodes.AadhaarSendOTP] =
                        json => Deserialize<AadhaarSendOtpRequest>(json),

                    [VerificationCodes.AadhaarVerifyOtp] =
                        json => Deserialize<AadhaarVerifyOtpRequest>(json),

                    /*[VerificationCodes.Passport] =
                        json => Deserialize<PassportVerifyRequest>(json),

                    [VerificationCodes.Ifsc] =
                        json => Deserialize<IfscLookupRequest>(json),

                    [VerificationCodes.BankAccount] =
                        json => Deserialize<BankAccountVerifyRequest>(json),

                    [VerificationCodes.Pincode] =
                        json => Deserialize<PincodeLookupRequest>(json),

                    [VerificationCodes.VehicleRc] =
                        json => Deserialize<VehicleRcRequest>(json),

                    [VerificationCodes.DrivingLicence] =
                        json => Deserialize<DrivingLicenceRequest>(json),

                    [VerificationCodes.VoterId] =
                        json => Deserialize<VoterIdRequest>(json),

                    [VerificationCodes.Uan] =
                        json => Deserialize<UanRequest>(json),

                    [VerificationCodes.Gst] =
                        json => Deserialize<GstVerifyRequest>(json),

                    [VerificationCodes.Cin] =
                        json => Deserialize<CinVerifyRequest>(json),

                    [VerificationCodes.Fssai] =
                        json => Deserialize<FssaiVerifyRequest>(json),

                    [VerificationCodes.PassbookOcr] =
                        json => Deserialize<PassbookOcrRequest>(json)*/

                        // Continue registering new common request DTOs here.
                };
        }

        public object? CreateRequest(
            string verificationCode,
            string? requestJson)
        {
            if (!_requestFactories.TryGetValue(
                    verificationCode,
                    out var factory))
            {
                throw new NotSupportedException(
                    $"Verification '{verificationCode}' is not registered.");
            }

            return factory(requestJson);
        }

        public object? CreateRequest(
            string verificationCode,
            JsonElement requestJson)
        {
            return CreateRequest(
                verificationCode,
                requestJson.GetRawText());
        }

        private static T? Deserialize<T>(
            string? json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(
                json);
        }
    }
}
