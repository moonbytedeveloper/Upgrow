using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.Verification.SprintVerify.Response;
using VerifyIndia.Application.Helper;

namespace VerifyIndia.Application.Verification
{
    public static class MockResponseProvider
    {
        public static ApiResponse<T> Get<T>(string endpoint)
            where T : class
        {
            return endpoint switch
            {
                ApiEndpoints.CENTER_VERIFY_PAN =>
                    ApiResponse<T>.Ok(
                        (T)(object)new SP_PanVerifyResponse
                        {
                            pan_number = "BXXFH1234F",
                            full_name = "Hardik",
                            dob = null,
                            gender = null,
                            category = "person"
                        },
                        "Success",
                        HttpStatusCode.OK),

                ApiEndpoints.CENTER_AADHAR_SEND_OTP =>
                    ApiResponse<T>.Ok(
                        (T)(object)new PS_AadharSendOtpResponse
                        {
                            client_id = Guid.NewGuid().ToString(),
                            otp_sent = true,
                            if_number = true,
                            valid_aadhaar = true,
                            status = "generate_otp_success"
                        },
                        "Success",
                        HttpStatusCode.OK),

                ApiEndpoints.CENTER_AADHAR_VERIFY_OTP =>
                    ApiResponse<T>.Ok(
                        (T)(object)new PS_AadhaarVerifyOtpResponse
                        {
                            client_id = "CLI123456789",
                            full_name = "Hardik Kangasiya",
                            aadhaar_number = "XXXX XXXX 1234",
                            dob = "01-01-2000",
                            gender = "M",

                            address = new Address
                            {
                                country = "India",
                                dist = "Ahmedabad",
                                state = "Gujarat",
                                po = "Navrangpura",
                                loc = "Navrangpura",
                                vtc = "Ahmedabad",
                                subdist = "Ahmedabad City",
                                street = "C.G. Road",
                                house = "101, Shree Complex",
                                landmark = "Near Municipal Market"
                            },

                            face_status = true,
                            face_score = 98,

                            zip = "380009",
                            profile_image = "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",

                            has_image = true,

                            email_hash = "e4b7d2c9f8a3e6d1",
                            mobile_hash = "9a7b6c5d4e3f2a1b",

                            raw_xml = "<PrintLetterBarcodeData uid=\"XXXXXXXX1234\" name=\"Hardik Kangasiya\" />",
                            zip_data = "base64_encoded_zip_data",

                            care_of = "S/O Bharatbhai Kangasiya",
                            share_code = "1234",

                            mobile_verified = true,

                            reference_id = "REF202607120001",

                            aadhaar_pdf = "base64_encoded_pdf",

                            status = "success",
                            uniqueness_id = Guid.NewGuid().ToString()
                        },
                        "Success",
                        HttpStatusCode.OK),

                _ => throw new NotSupportedException(
                    $"Mock response not implemented for endpoint '{endpoint}'.")
            };
        }
    }
}
