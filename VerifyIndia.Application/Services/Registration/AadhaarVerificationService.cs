using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.Registration;
using VerifyIndia.Application.Helper;
using VerifyIndia.Application.Interfaces.Registration;

namespace VerifyIndia.Application.Services.Registration
{
    public class AadhaarVerificationService
        : IAadhaarVerificationService
    {
        private readonly CentralApiClient
            _centralApiClient;

        private readonly IConfiguration
            _configuration;

        public AadhaarVerificationService(
            IConfiguration configuration,
            CentralApiClient centralApiClient)
        {
            _configuration =
                configuration;

            _centralApiClient =
                centralApiClient;
        }

        public async Task<
            SendAadhaarOtpResponseDto>
            SendOtpAsync(
                string aadhaarNumber)
        {
            if (_configuration.GetValue<bool>("SprintVerifyProvider:UseMock"))
            {
                return new SendAadhaarOtpResponseDto
                {
                    Success = true,
                    ClientId = "MOCK_CLIENT_123456",
                    Message = "Mock OTP sent successfully."
                };
            }

            var response =
                await _centralApiClient
                    .PostAsync<
                        AadhaarSendOtpApiResponse>(
                        "AadharSendOTP",
                        new
                        {
                            id_number =
                                aadhaarNumber
                        });

            if (!response.Success)
            {
                throw new Exception(
                    response.Message);
            }

            return new SendAadhaarOtpResponseDto
            {
                Success = true,

                ClientId =
                    response.Data!
                        .ClientId,

                Message =
                    response.Message
            };


        }

        public async Task<
            VerifyAadhaarOtpResponseDto>
            VerifyOtpAsync(
                string refId,
                string clientId,
                string otp)
        {
            if (_configuration.GetValue<bool>("SprintVerifyProvider:UseMock"))
            {
                return new VerifyAadhaarOtpResponseDto
                {
                    Success = true,
                    FullName = "Hardik Kangasiya",
                    AadhaarNumber = "1234567891234",
                    ReferenceId = refId
                };
            }

            var response =
                await _centralApiClient
                    .PostAsync<
                        AadhaarVerifyOtpApiResponse>(
                        "AadharVerifyOTP",
                        new
                        {
                            refid = refId,
                            client_id = clientId,
                            otp = otp
                        });

            if (!response.Success)
            {
                throw new Exception(
                    response.Message);
            }

            return new VerifyAadhaarOtpResponseDto
            {
                Success = true,

                FullName =
                    response.Data!
                        .FullName,

                AadhaarNumber =
                    response.Data
                        .AadhaarNumber,

                ReferenceId =
                    response.Data
                        .ReferenceId
            };

            
        }
    }
}
