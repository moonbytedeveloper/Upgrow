using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.Verification.Common.Request;
using VerifyIndia.Application.DTO.Verification.Common.Response;
using VerifyIndia.Application.DTO.Verification.SprintVerify;
using VerifyIndia.Application.DTO.Verification.SprintVerify.Response;
using VerifyIndia.Application.Helper;
using VerifyIndia.Application.Verification;
using VerifyIndia.Application.Verification.Interfaces;
using static VerifyIndia.Application.Constants;

namespace VerifyIndia.Infrastructure.Verification.Providers.SprintVerify
{
    public static class SprintDefinitions
    {
        public static readonly IReadOnlyDictionary<string, IVerificationDefinition> All =
            new Dictionary<string, IVerificationDefinition>(
                StringComparer.OrdinalIgnoreCase)
            {
                [VerificationCodes.Pan] =
                    new VerificationDefinition<
                        PanVerifyRequest,
                        PanVerifyRequestDto,
                        PanVerifyResponse,
                        SP_PanVerifyResponse>
                    {
                        VerificationCode = VerificationCodes.Pan,

                        Endpoint = ApiEndpoints.CENTER_VERIFY_PAN,

                        LogMessage = "[SprintProvider] PAN request mapped: {Pan}",

                        LogValueSelector = providerRequest => providerRequest.pannumber,

                        ResponseMapper = (response, result) =>
                        {
                            result.FullName = response.full_name;
                            result.PrimaryIdentifier = response.pan_number;
                        }
                    },

                [VerificationCodes.AadhaarSendOTP] =
                    new VerificationDefinition<
                        AadhaarSendOtpRequest,
                        AadharSendOtpDto,
                        AadharSendOtpResponse,
                        PS_AadharSendOtpResponse>
                    {
                        VerificationCode = VerificationCodes.AadhaarSendOTP,

                        Endpoint = ApiEndpoints.CENTER_AADHAR_SEND_OTP,

                        LogMessage = "[SprintProvider] Aadhaar request mapped: {IdNumber}",

                        LogValueSelector = providerRequest => providerRequest.id_number,

                        ResponseMapper = (response, result) =>
                        {
                            result.RequiresUserInput = true;
                            result.NextVerificationCode =
                                VerificationCodes.AadhaarVerifyOtp;
                        }
                    },

                [VerificationCodes.AadhaarVerifyOtp] =
                    new VerificationDefinition<
                        AadhaarVerifyOtpRequest,
                        AadharVerifyOtpDto,
                        AadhaarVerifyOtpResponse,
                        PS_AadhaarVerifyOtpResponse>
                    {
                        VerificationCode = VerificationCodes.AadhaarVerifyOtp,

                        Endpoint = ApiEndpoints.CENTER_AADHAR_VERIFY_OTP,

                        LogMessage = "[SprintProvider] Aadhaar request mapped: {ClientId}",

                        LogValueSelector = providerRequest => providerRequest.client_id,

                        ResponseMapper = (response, result) =>
                        {
                            result.FullName = response.full_name;
                            result.PrimaryIdentifier = response.aadhaar_number;
                        }
                    },

                [VerificationCodes.DlV2] =
                    new VerificationDefinition<
                        DrivingLicenceRequest,
                        DrivingLicenceV1Dto,
                        DrivingLicenceResponse,
                        PS_DrivingLicenceResponse>
                    {
                        VerificationCode = VerificationCodes.DlV2,

                        Endpoint = ApiEndpoints.CENTER_DRIVING_LICENCE_V1,

                        LogMessage = "[SprintProvider] DL V2 request mapped: {DrivingLicenceNumber}",

                        LogValueSelector = providerRequest => providerRequest.id_number
                    },

                [VerificationCodes.Voter] =
                    new VerificationDefinition<
                        VoterValidateRequest,
                        VoterValidateDto,
                        VoterValidateResponse,
                        PS_VoterValidateResponse>
                    {
                        VerificationCode = VerificationCodes.Voter,

                        Endpoint = ApiEndpoints.CENTER_VALIDATE_VOTER_ID,

                        LogMessage = "[SprintProvider] Voter request mapped: {IdNumber}",

                        LogValueSelector = providerRequest => providerRequest.id_number
                    },

                [VerificationCodes.Passport] =
                    new VerificationDefinition<
                        PassportVerifyRequest,
                        PassportVerifyDto,
                        PassportVerifyResponse,
                        PS_PassportVerifyResponse>
                    {
                        VerificationCode = VerificationCodes.Passport,

                        Endpoint = ApiEndpoints.CENTER_GST_PASSPORT,

                        LogMessage = "[SprintProvider] Passport request mapped: {IdNumber}",

                        LogValueSelector = providerRequest => providerRequest.id_number
                    },

                [VerificationCodes.Tan] =
                    new VerificationDefinition<
                        TANRequest,
                        TANDto,
                        TANResponse,
                        PS_TANResponse>
                    {
                        VerificationCode = VerificationCodes.Tan,

                        Endpoint = ApiEndpoints.CENTER_TAN_VERIFY,

                        LogMessage = "[SprintProvider] TAN request mapped: {IdNumber}",

                        LogValueSelector = providerRequest => providerRequest.id_number
                    },

                [VerificationCodes.Rc] =
                    new VerificationDefinition<
                        RCVerifyRequest,
                        RCVerifyDto,
                        RCVerifyResponse,
                        PS_RCVerifyResponse>
                    {
                        VerificationCode = VerificationCodes.Rc,

                        Endpoint = ApiEndpoints.CENTER_RC_VERIFICATION,

                        LogMessage = "[SprintProvider] RC request mapped: {IdNumber}",

                        LogValueSelector = providerRequest => providerRequest.id_number
                    },

                [VerificationCodes.Iec] =
                    new VerificationDefinition<
                        IECVerifyRequest,
                        IECVerifyDto,
                        IECVerifyResponse,
                        PS_IECVerifyResponse>
                    {
                        VerificationCode = VerificationCodes.Iec,

                        Endpoint = ApiEndpoints.CENTER_IEC_VERIFICATION,

                        LogMessage = "[SprintProvider] IEC request mapped: {IecNumber}",

                        LogValueSelector = providerRequest => providerRequest.iec_number
                    },

                [VerificationCodes.Fssai] =
                    new VerificationDefinition<
                        FSSAIRequest,
                        FSSAIDto,
                        FSSAIResponse,
                        PS_FSSAIResponse>
                    {
                        VerificationCode = VerificationCodes.Fssai,

                        Endpoint = ApiEndpoints.CENTER_IFSSAI_VERIFICATION,

                        LogMessage = "[SprintProvider] FSSAI request mapped: {FssaiNumber}",

                        LogValueSelector = providerRequest => providerRequest.fssai_number
                    },

                [VerificationCodes.IfscLookup] =
                new VerificationDefinition<
                    IfscVerifyRequest,
                    IFSCLookupRequestDto,
                    IfscVerifyResponse,
                    PS_IfscVerifyResponse>
                {
                    VerificationCode = VerificationCodes.IfscLookup,

                    Endpoint = ApiEndpoints.CENTER_IFSC_LOOKUP,

                    LogMessage = "[SprintProvider] IFSC request mapped: {IfscCode}",

                    LogValueSelector = providerRequest => providerRequest.ifsc
                },

                [VerificationCodes.PincodeInfo] =
                new VerificationDefinition<
                    PincodeInfoRequest,
                    PincodeInfoRequestDto,
                    PincodeInfoResponse,
                    PS_PincodeInfoResponse>
                {
                    VerificationCode = VerificationCodes.PincodeInfo,

                    Endpoint = ApiEndpoints.CENTER_PINCODE_INFO,

                    LogMessage = "[SprintProvider] Pincode request mapped: {Pincode}",

                    LogValueSelector = providerRequest => providerRequest.pincode
                },

                [VerificationCodes.IpLookup] =
                new VerificationDefinition<
                    IplookupRequest,
                    IplookupDto,
                    IplookupResponse,
                    PS_IplookupResponse>
                {
                    VerificationCode = VerificationCodes.IpLookup,

                    Endpoint = ApiEndpoints.CENTER_IP_LOOKUP,

                    LogMessage = "[SprintProvider] IP request mapped: {IpAddress}",

                    LogValueSelector = providerRequest => providerRequest.ip_address
                },

                [VerificationCodes.MobileOperatorCheck] =
                new VerificationDefinition<
                    MobileOperatorRequest,
                    MobileOperatorDto,
                    MobileOperatorResponse,
                    PS_MobileOperatorResponse>
                {
                    VerificationCode = VerificationCodes.MobileOperatorCheck,

                    Endpoint = ApiEndpoints.CENTER_MOBILE_OPERATOR_CHECK,

                    LogMessage = "[SprintProvider] Mobile operator request mapped: {MobileNumber}",

                    LogValueSelector = providerRequest => providerRequest.mobile
                },

                [VerificationCodes.EmailChecker] =
                new VerificationDefinition<
                    EmailCheckerRequest,
                    EmailCheckerDto,
                    EmailCheckerResponse,
                    PS_EmailCheckerResponse>
                {
                    VerificationCode = VerificationCodes.EmailChecker,

                    Endpoint = ApiEndpoints.CENTER_EMAIL_CHECKER,

                    LogMessage = "[SprintProvider] Email request mapped: {Email}",

                    LogValueSelector = providerRequest => providerRequest.email
                },

                [VerificationCodes.VpnProxyCheck] =
                new VerificationDefinition<
                    VpnProxyCheckRequest,
                    VpnProxyCheckRequestDto,
                    VpnProxyCheckResponse,
                    PS_VpnProxyCheckResponse>
                {
                    VerificationCode = VerificationCodes.VpnProxyCheck,

                    Endpoint = ApiEndpoints.CENTER_VPN_PROXY_CHECK,

                    LogMessage = "[SprintProvider] VPN proxy request mapped: {IpAddress}",

                    LogValueSelector = providerRequest => providerRequest.ip_address
                },

                [VerificationCodes.CardValidation] =
                new VerificationDefinition<
                    CardValidationRequest,
                    CardValidationDto,
                    CardValidationResponse,
                    PS_CardValidationResponse>
                {
                    VerificationCode = VerificationCodes.CardValidation,

                    Endpoint = ApiEndpoints.CENTER_CARD_VALIDATION,

                    LogMessage = "[SprintProvider] Card validation request mapped: {CardNumber}",

                    LogValueSelector = providerRequest => providerRequest.card_num
                },

                [VerificationCodes.PanToGst] =
                new VerificationDefinition<
                    PanToGstRequest,
                    PanToGstDto,
                    PanToGstResponse,
                    PS_PanToGstResponse>
                {
                    VerificationCode = VerificationCodes.PanToGst,

                    Endpoint = ApiEndpoints.CENTER_PAN_TO_GST,

                    LogMessage = "[SprintProvider] PAN to GST request mapped: {PanNumber}",

                    LogValueSelector = providerRequest => providerRequest.pan_number
                },

                [VerificationCodes.CompanyToTan] =
                new VerificationDefinition<
                    TanLookupRequest,
                    TanLookupDto,
                    TanLookupResponse,
                    PS_TanLookupResponse>
                {
                    VerificationCode = VerificationCodes.CompanyToTan,

                    Endpoint = ApiEndpoints.CENTER_TAN_LOOKUP,

                    LogMessage = "[SprintProvider] Company to TAN request mapped: {CompanyName}",

                    LogValueSelector = providerRequest => providerRequest.company_name
                },

                [VerificationCodes.Din] =
                new VerificationDefinition<
                    DinVerificationRequest,
                    DinVerificationRequestDto,
                    DinVerificationResponse,
                    PS_DinVerificationResponse>
                {
                    VerificationCode = VerificationCodes.Din,

                    Endpoint = ApiEndpoints.CENTER_DIN_VERIFICATION,

                    LogMessage = "[SprintProvider] DIN to mobile request mapped: {DinNumber}",

                    LogValueSelector = providerRequest => providerRequest.din_number
                },

                [VerificationCodes.DinToMobileNumber] =
                new VerificationDefinition<
                    DinToMobileNumberRequest,
                    DinToMobileNumberRequestDto,
                    DinToMobileNumberResponse,
                    PS_DinToMobileNumberResponse>
                {
                    VerificationCode = VerificationCodes.DinToMobileNumber,

                    Endpoint = ApiEndpoints.CENTER_MOBILE_VERIFY,

                    LogMessage = "[SprintProvider] DIN to mobile request mapped: {DinNumber}",

                    LogValueSelector = providerRequest => providerRequest.din_number
                },

                [VerificationCodes.MobileToUpiVpa] =
                new VerificationDefinition<
                    MobileToUpiVpaRequest,
                    MobileToUpiVpaDto,
                    MobileToUpiVpaResponse,
                    PS_MobileToUpiVpaResponse>
                {
                    VerificationCode = VerificationCodes.MobileToUpiVpa,

                    Endpoint = ApiEndpoints.CENTER_MOBILE_TO_UPI_VPA,

                    LogMessage = "[SprintProvider] Mobile to UPI request mapped: {MobileNumber}",

                    LogValueSelector = providerRequest => providerRequest.mobile
                },

                [VerificationCodes.LeiVerification] =
                new VerificationDefinition<
                    LEIVerifyRequest,
                    LEIVerifyDto,
                    LEIVerifyResponse,
                    PS_LEIVerifyResponse>
                {
                    VerificationCode = VerificationCodes.LeiVerification,

                    Endpoint = ApiEndpoints.CENTER_LEI_VERIFY,

                    LogMessage = "[SprintProvider] LEI request mapped: {IdNumber}",

                    LogValueSelector = providerRequest => providerRequest.id_number
                },

                [VerificationCodes.RtoInformation] =
                new VerificationDefinition<
                    RtoInformationRequest,
                    RtoInformationDto,
                    RtoInformationResponse,
                    PS_RtoInformationResponse>
                {
                    VerificationCode = VerificationCodes.RtoInformation,

                    Endpoint = ApiEndpoints.CENTER_RTO_INFORMATION,

                    LogMessage = "[SprintProvider] RTO request mapped: {RtoCode}",

                    LogValueSelector = providerRequest => providerRequest.rto_code
                },

                [VerificationCodes.StockPrice] =
                new VerificationDefinition<
                    StockPriceVerificationRequest,
                    StockPriceVerificationDto,
                    StockPriceVerificationResponse,
                    PS_StockPriceVerificationResponse>
                {
                    VerificationCode = VerificationCodes.StockPrice,

                    Endpoint = ApiEndpoints.CENTER_STOCK_PRICE_VERIFICATION,

                    LogMessage = "[SprintProvider] Stock request mapped: {Company}",

                    LogValueSelector = providerRequest => providerRequest.company
                },

                [VerificationCodes.ReverseGeolocation] =
                new VerificationDefinition<
                    ReverseGeoLocationRequest,
                    ReverseGeoLocationDto,
                    ReverseGeoLocationResponse,
                    PS_ReverseGeoLocationResponse>
                {
                    VerificationCode = VerificationCodes.ReverseGeolocation,

                    Endpoint = ApiEndpoints.CENTER_REVERSE_GEO_LOCATION,

                    LogMessage = "[SprintProvider] Reverse geolocation request mapped: {Latitude},{Longitude}",

                    LogValueSelector = providerRequest =>
                        $"{providerRequest.latitude},{providerRequest.longitude}"
                },

                [VerificationCodes.FuelPrice] =
                new VerificationDefinition<
                    FuelPriceFetchRequest,
                    FuelPriceFetchDto,
                    FuelPriceFetchResponse,
                    PS_FuelPriceFetchResponse>
                {
                    VerificationCode = VerificationCodes.FuelPrice,

                    Endpoint = ApiEndpoints.CENTER_FUEL_PRICE_FETCH,

                    LogMessage = "[SprintProvider] Fuel price request mapped: {CityState}",

                    LogValueSelector = providerRequest => providerRequest.citystate
                },

                [VerificationCodes.NameMatch] =
                new VerificationDefinition<
                    NameMatchRequest,
                    NameMatchDto,
                    NameMatchResponse,
                    PS_NameMatchResponse>
                {
                    VerificationCode = VerificationCodes.NameMatch,

                    Endpoint = ApiEndpoints.CENTER_NAME_MATCH,

                    LogMessage = "[SprintProvider] Name match request mapped: {PrimaryName}",

                    LogValueSelector = providerRequest => providerRequest.name_1
                },

                [VerificationCodes.AadhaarQrCheck] =
                new VerificationDefinition<
                    AadharQRCheckRequest,
                    AdhaarQRCheckDto,
                    PS_AadharQRCheckResponse,
                    AadharQRCheckResponse>
                {
                    VerificationCode = VerificationCodes.AadhaarQrCheck,

                    Endpoint = ApiEndpoints.CENTER_QR_CHECK,

                    LogMessage = "[SprintProvider] Aadhaar QR request mapped.",

                    LogValueSelector = static _ => "QR"
                },

                [VerificationCodes.UpiVerification] =
                new VerificationDefinition<
                    UPIRequest,
                    UPIDto,
                    UPIResponse,
                    PS_UPIResponse>
                {
                    VerificationCode = VerificationCodes.UpiVerification,

                    Endpoint = ApiEndpoints.CENTER_UPI_VERIFY,

                    LogMessage = "[SprintProvider] UPI request mapped: {IdNumber}",

                    LogValueSelector = providerRequest => providerRequest.id_number
                },

                [VerificationCodes.FaceMatch] =
                new VerificationDefinition<
                    FaceMatchRequest,
                    FaceMatchDto,
                    FaceMatchResponse,
                    PS_FaceMatchResponse>
                {
                    VerificationCode = VerificationCodes.FaceMatch,

                    Endpoint = ApiEndpoints.CENTER_FACE_MATCH_VERIFY,

                    LogMessage = "[SprintProvider] Face match request mapped.",

                    LogValueSelector = static _ => "FACE_MATCH"
                },

                [VerificationCodes.LivenessCheck] =
                new VerificationDefinition<
                    LivenessRequest,
                    LivenessDto,
                    LivenessResponse,
                    PS_LivenessResponse>
                {
                    VerificationCode = VerificationCodes.LivenessCheck,

                    Endpoint = ApiEndpoints.CENTER_LIVENESS_CHECK,

                    LogMessage = "[SprintProvider] Liveness request mapped.",

                    LogValueSelector = static _ => "LIVENESS_CHECK"
                },

                [VerificationCodes.GstAdvance] =
                new VerificationDefinition<
                    GstAdvancev2Request,
                    GstAdvanceV2Dto,
                    GstAdvancev2Response,
                    PS_GstAdvancev2Response>
                {
                    VerificationCode = VerificationCodes.GstAdvance,

                    Endpoint = ApiEndpoints.CENTER_GST_ADVANCE_V2,

                    LogMessage = "[SprintProvider] GST advance request mapped: {GstNumber}",

                    LogValueSelector = providerRequest => providerRequest.gst_number
                },

                [VerificationCodes.TanAdvance] =
                new VerificationDefinition<
                    TANAdvanceRequest,
                    TANAdvanceDto,
                    TANAdvanceResponse,
                    PS_TANAdvanceResponse>
                {
                    VerificationCode = VerificationCodes.TanAdvance,

                    Endpoint = ApiEndpoints.CENTER_TAN_ADVANCE_VERIFY,

                    LogMessage = "[SprintProvider] TAN advance request mapped: {TanNumber}",

                    LogValueSelector = providerRequest => providerRequest.tan_number
                },

                [VerificationCodes.UdyamAadhaarV2] =
                new VerificationDefinition<
                    UdyamAadhaarRequest,
                    UdyamAadhaarV1Dto,
                    UdyamAadhaarResponse,
                    PS_UdyamAadhaarResponse>
                {
                    VerificationCode = VerificationCodes.UdyamAadhaarV2,

                    Endpoint = ApiEndpoints.CENTER_UDYAM_AADHAAR_V1,

                    LogMessage = "[SprintProvider] Udyam Aadhaar V1 request mapped: {id_number}",

                    LogValueSelector = providerRequest => providerRequest.id_number
                },

                [VerificationCodes.RcAdvance] =
                new VerificationDefinition<
                    RcAdvanceVerifyRequest,
                    RCAdvanceVerifyDto,
                    RcAdvanceVerifyRequest,
                    PS_RcAdvanceVerifyResponse>
                {
                    VerificationCode = VerificationCodes.RcAdvance,

                    Endpoint = ApiEndpoints.CENTER_RC_ADVANCE_VERIFICATION,

                    LogMessage = "[SprintProvider] RC advance request mapped: {RcNumber}",

                    LogValueSelector = providerRequest => providerRequest.rc_number
                },

                [VerificationCodes.VehicleChallanV2] =
                new VerificationDefinition<
                    VehicalChallanRequest,
                    VehicalChallanV2Dto,
                    VehicalChallanResponse,
                    PS_VehicalChallanResponse>
                {
                    VerificationCode = VerificationCodes.VehicleChallanV2,

                    Endpoint = ApiEndpoints.CENTER_VEHICLE_CHALLAN_V2,

                    LogMessage = "[SprintProvider] Vehicle challan V2 request mapped: {RcNumber}",

                    LogValueSelector = providerRequest => providerRequest.rc_number
                },

                [VerificationCodes.McaVerify] =
                new VerificationDefinition<
                    MCARequest,
                    MCADto,
                    MCAResponse,
                    PS_MCAResponse>
                {
                    VerificationCode = VerificationCodes.McaVerify,

                    Endpoint = ApiEndpoints.CENTER_MCA_VERIFICATION,

                    LogMessage = "[SprintProvider] MCA request mapped: {IdNumber}",

                    LogValueSelector = providerRequest => providerRequest.id_number
                },

                [VerificationCodes.ItrComplianceCheck] =
                new VerificationDefinition<
                    ITRCompliancecheckRequest,
                    ITRCompliancecheckDto,
                    ITRCompliancecheckResponse,
                    PS_ITRCompliancecheckResponse>
                {
                    VerificationCode = VerificationCodes.ItrComplianceCheck,

                    Endpoint = ApiEndpoints.CENTER_INCOME_TAX_RETURN,

                    LogMessage = "[SprintProvider] ITR compliance request mapped: {PanNumber}",

                    LogValueSelector = providerRequest => providerRequest.pan_number
                },

                [VerificationCodes.CheckStatus] =
                new VerificationDefinition<
                    CommonStatusCheckRequest,
                    CommonStatusCheckRequestDto,
                    CommonStatusCheckResponse,
                    PS_CommonStatusCheckResponse>
                {
                    VerificationCode = VerificationCodes.CheckStatus,

                    Endpoint = ApiEndpoints.CENTER_STATUS_CHECK,

                    LogMessage = "[SprintProvider] Check status request mapped: {ReferenceId}",

                    LogValueSelector = providerRequest => providerRequest.refid
                },

                [VerificationCodes.CompanyNameToCin] =
                new VerificationDefinition<
                    CompanyNameToCinRequest,
                    CompanyNameToCinRequestDto,
                    CompanyNameToCinResponse,
                    PS_CompanyNameToCinResponse>
                {
                    VerificationCode = VerificationCodes.CompanyNameToCin,

                    Endpoint = ApiEndpoints.CENTER_CONVERT,

                    LogMessage = "[SprintProvider] Company to CIN request mapped: {CompanyName}",

                    LogValueSelector = providerRequest => providerRequest.CompanyName
                },

                [VerificationCodes.MobileIntelligence] =
                new VerificationDefinition<
                    MobileIntelligenceRequest,
                    MobileIntelligenceRequestDto,
                    MobileIntelligenceResponse,
                    PS_MobileIntelligenceResponse>
                {
                    VerificationCode = VerificationCodes.MobileIntelligence,

                    Endpoint = ApiEndpoints.CENTER_MOBILE_INTELLIGENCE,

                    LogMessage = "[SprintProvider] Mobile intelligence request mapped: {MobileNumber}",

                    LogValueSelector = providerRequest => providerRequest.phone
                },

                [VerificationCodes.TelecomVerifyOtp] =
                new VerificationDefinition<
                    TelecomValidateRequest,
                    TelecomValidateDto,
                    TelecomValidateResponse,
                    PS_TelecomValidateResponse>
                {
                    VerificationCode = VerificationCodes.TelecomVerifyOtp,

                    Endpoint = ApiEndpoints.CENTER_TELECOM_VALIDATE,

                    LogMessage = "[SprintProvider] Telecom send OTP request mapped: {IdNumber}",

                    LogValueSelector = providerRequest => providerRequest.id_number
                },

                [VerificationCodes.TelecomSendOtp] =
                new VerificationDefinition<
                    TelecomDetailRequest,
                    TelecomDetailDto,
                    TelecomDetailResponse,
                    PS_TelecomDetailResponse>
                {
                    VerificationCode = VerificationCodes.TelecomSendOtp,

                    Endpoint = ApiEndpoints.CENTER_TELECOM_GET_DETAILS,

                    LogMessage = "[SprintProvider] Telecom verify OTP request mapped: {ClientId}",

                    LogValueSelector = providerRequest => providerRequest.client_id
                },

                [VerificationCodes.SendOtp] =
                new VerificationDefinition<
                    OtpSendRequest,
                    OTPSendDto,
                    OtpSendResponse,
                    PS_OtpSendResponse>
                {
                    VerificationCode = VerificationCodes.SendOtp,

                    Endpoint = ApiEndpoints.CENTER_OTP_SEND,

                    LogMessage = "[SprintProvider] Send OTP request mapped: {EpfoNumber}",

                    LogValueSelector = providerRequest => providerRequest.epfo_number
                },

                [VerificationCodes.VerifyOtp] =
                new VerificationDefinition<
                    OTPVerifyRequest,
                    OTPVerifyDto,
                    OTPVerifyResponse,
                    PS_OTPVerifyResponse>
                {
                    VerificationCode = VerificationCodes.VerifyOtp,

                    Endpoint = ApiEndpoints.CENTER_OTP_VERIFY,

                    LogMessage = "[SprintProvider] Verify OTP request mapped: {OTP}",

                    LogValueSelector = providerRequest => providerRequest.otp
                },

                [VerificationCodes.CrimeCheckIndividual] =
                new VerificationDefinition<
                    CrimeCheckIndividualRequest,
                    CrimeCheckIndividualRequestDto,
                    CrimeCheckIndividualResponse,
                    PS_CrimeCheckIndividualResponse>
                {
                    VerificationCode = VerificationCodes.CrimeCheckIndividual,

                    Endpoint = ApiEndpoints.CENTER_CRIME_CHECK_INDIVIDUAL,

                    LogMessage = "[SprintProvider] Crime check individual request mapped: {Name}",

                    LogValueSelector = providerRequest => providerRequest.Name
                },

                [VerificationCodes.CrimeCheckCompany] =
                new VerificationDefinition<
                    CrimeCheckCompanyRequest,
                    CrimeCheckCompanyRequestDto,
                    CrimeCheckCompanyResponse,
                    PS_CrimeCheckIndividualResponse>
                {
                    VerificationCode = VerificationCodes.CrimeCheckCompany,

                    Endpoint = ApiEndpoints.CENTER_CRIME_CHECK_COMPANY,

                    LogMessage = "[SprintProvider] Crime check company request mapped: {CompanyName}",

                    LogValueSelector = providerRequest => providerRequest.CompanyName
                },

                [VerificationCodes.CrimeCheckDownloadPdfReport] =
                new VerificationDefinition<
                    CrimeCheckDownloadPdfReportRequest,
                    CrimeCheckDownloadPdfReportRequestDto,
                    CrimeCheckDownloadPdfReportResponse,
                    PS_CrimeCheckDownloadPdfReportResponse>
                {
                    VerificationCode = VerificationCodes.CrimeCheckDownloadPdfReport,

                    Endpoint = ApiEndpoints.CENTER_DOWNLOAD_PDF_REPORT,

                    LogMessage = "[SprintProvider] Crime check PDF report request mapped: {RequestId}",

                    LogValueSelector = providerRequest => providerRequest.RequestId
                },

                [VerificationCodes.CrimeCheckDownloadJsonReport] =
                new VerificationDefinition<
                    CrimeCheckDownloadJsonReportRequest,
                    CrimeCheckDownloadJsonReportRequestDto,
                    CrimeCheckDownloadJsonReportResponse,
                    PS_CrimeCheckDownloadJsonReportResponse>
                {
                    VerificationCode = VerificationCodes.CrimeCheckDownloadJsonReport,

                    Endpoint = ApiEndpoints.CENTER_DOWNLOAD_JSON_REPORT,

                    LogMessage = "[SprintProvider] Crime check JSON report request mapped: {RequestId}",

                    LogValueSelector = providerRequest => providerRequest.RequestId
                },

                [VerificationCodes.CourtCaseStatus] =
                new VerificationDefinition<
                    CourtCaseStatusRequest,
                    CourtCaseStatusDto,
                    CourtCaseStatusResponse,
                    PS_CourtCaseStatusResponse>
                {
                    VerificationCode = VerificationCodes.CourtCaseStatus,

                    Endpoint = ApiEndpoints.CENTER_COURT_CASE_STATUS,

                    LogMessage = "[SprintProvider] Court case status request mapped: {Name}",

                    LogValueSelector = providerRequest => providerRequest.name
                },

                [VerificationCodes.BavHybridV3] =
                new VerificationDefinition<
                    BAVHybridRequest,
                    BAVHybridV3,
                    BAVHybridResponse,
                    PS_BAVHybridResponse>
                {
                    VerificationCode = VerificationCodes.BavHybridV3,

                    Endpoint = ApiEndpoints.CENTER_BAV_PENNYLESS_V3,

                    LogMessage = "[SprintProvider] BAV Hybrid V3 request mapped: {AccountNumber}",

                    LogValueSelector = providerRequest => providerRequest.account_number
                },

                [VerificationCodes.BavPennylessV3] =
                new VerificationDefinition<
                    PennyLessVerifyRequest,
                    BAVPennylessV3,
                    PennyLessVerifyResponse,
                    PS_PennyLessVerifyResponse>
                {
                    VerificationCode = VerificationCodes.BavPennylessV3,

                    Endpoint = ApiEndpoints.CENTER_BAV_PENNYLESS_V3,

                    LogMessage = "[SprintProvider] BAV Pennyless V3 request mapped: {AccountNumber}",

                    LogValueSelector = providerRequest => providerRequest.account_number
                },

                [VerificationCodes.BavPennydropV2] =
                new VerificationDefinition<
                    PennyDropVerifyRequest,
                    BankAccountVerification,
                    PennyDropVerifyResponse,
                    PS_PennyDropVerifyResponse>
                {
                    VerificationCode = VerificationCodes.BavPennydropV2,

                    Endpoint = ApiEndpoints.CENTER_BAV_PENNYPDROP_V2,

                    LogMessage = "[SprintProvider] BAV PennyDrop V2 request mapped: {AccountNumber}",

                    LogValueSelector = providerRequest => providerRequest.account_number
                },

                [VerificationCodes.DigilockerInitiateSession] =
                new VerificationDefinition<
                    DigilockerInitiateSessionRequest,
                    DigilockerInitiateSessionRequestDto,
                    DigilockerInitiateSessionResponse,
                    PS_DigilockerInitiateSessionResponse>
                {
                    VerificationCode = VerificationCodes.DigilockerInitiateSession,

                    Endpoint = ApiEndpoints.CENTER_INITIATE_SESSION,

                    LogMessage = "[SprintProvider] DigiLocker initiate session request mapped.",

                    LogValueSelector = static _ => "DIGILOCKER_SESSION"
                },

                [VerificationCodes.DigilockerAccessToken] =
                new VerificationDefinition<
                    DigiLockerAccessTokenRequest,
                    DigilockerAccessTokenRequestDto,
                    DigiLockerAccessTokenResponse,
                    PS_DigiLockerAccessTokenResponse>
                {
                    VerificationCode = VerificationCodes.DigilockerAccessToken,

                    Endpoint = ApiEndpoints.CENTER_GENERATE_ACCESS_TOKEN,

                    LogMessage = "[SprintProvider] DigiLocker access token request mapped.",

                    LogValueSelector = static _ => "DIGILOCKER_ACCESS_TOKEN"
                },

                [VerificationCodes.DigilockerIssuedFiles] =
                new VerificationDefinition<
                    DigilockerIssuedFilesRequest,
                    DigilockerIssuedFilesRequestDto,
                    DigilockerIssuedFilesResponse,
                    PS_DigilockerIssuedFilesResponse>
                {
                    VerificationCode = VerificationCodes.DigilockerIssuedFiles,

                    Endpoint = ApiEndpoints.CENTER_GET_ISSUED_FILES,

                    LogMessage = "[SprintProvider] DigiLocker issued files request mapped.",

                    LogValueSelector = static _ => "DIGILOCKER_ISSUED_FILES"
                },

                [VerificationCodes.DigilockerDownloadPdf] =
                new VerificationDefinition<
                    DigilockerDownloadPdfRequest,
                    DigilockerDownloadPdfRequestDto,
                    DigilockerDownloadPdfResponse,
                    PS_DigilockerDownloadPdfResponse>
                {
                    VerificationCode = VerificationCodes.DigilockerDownloadPdf,

                    Endpoint = ApiEndpoints.CENTER_DOWNLOAD_DOCUMENT_PDF,

                    LogMessage = "[SprintProvider] DigiLocker download PDF request mapped: {Uri}",

                    LogValueSelector = providerRequest => providerRequest.Uri
                },

                [VerificationCodes.DigilockerDownloadXml] =
                new VerificationDefinition<
                    DigilockerDownloadXmlRequest,
                    DigilockerDownloadXmlRequestDto,
                    DigilockerDownloadXmlResponse,
                    PS_DigilockerDownloadXmlResponse>
                {
                    VerificationCode = VerificationCodes.DigilockerDownloadXml,

                    Endpoint = ApiEndpoints.CENTER_DOWNLOAD_DOCUMENT_XML,

                    LogMessage = "[SprintProvider] DigiLocker download XML request mapped: {Uri}",

                    LogValueSelector = providerRequest => providerRequest.Uri
                },

                [VerificationCodes.DigilockerEaadhaar] =
                new VerificationDefinition<
                    DigilockerEaadhaarRequest,
                    DigilockerEaadhaarRequestDto,
                    DigilockerEaadhaarResponse,
                    PS_DigilockerEaadhaarResponse>
                {
                    VerificationCode = VerificationCodes.DigilockerEaadhaar,

                    Endpoint = ApiEndpoints.CENTER_GET_EAADHAAR_XML,

                    LogMessage = "[SprintProvider] DigiLocker eAadhaar request mapped.",

                    LogValueSelector = static _ => "EAADHAAR"
                },

                [VerificationCodes.TwentySixAsDetails] =
                new VerificationDefinition<
                    SingleTwentySixASDetailsRequest,
                    SingleTwentySixASDetailsDto,
                    SingleTwentySixASDetailsResponse,
                    PS_SingleTwentySixASDetailsResponse>
                {
                    VerificationCode = VerificationCodes.TwentySixAsDetails,

                    Endpoint = ApiEndpoints.CENTER_SINGLE_26AS_DETAILS,

                    LogMessage = "[SprintProvider] 26AS details request mapped: {ReferenceId}",

                    LogValueSelector = providerRequest => providerRequest.refid
                },

                [VerificationCodes.TwentySixAsList] =
                new VerificationDefinition<
                    TwentySixASListRequest,
                    TwentySixASListDto,
                    TwentySixASListResponse,
                    PS_TwentySixASListResponse>
                {
                    VerificationCode = VerificationCodes.TwentySixAsList,

                    Endpoint = ApiEndpoints.CENTER_26AS_LIST,

                    LogMessage = "[SprintProvider] 26AS list request mapped: {ClientId}",

                    LogValueSelector = providerRequest => providerRequest.client_id
                },

                [VerificationCodes.ItrDetails] =
                new VerificationDefinition<
                    SingleItrDetailsRequest,
                    SingleItrDetailsDto,
                    SingleItrDetailsResponse,
                    PS_SingleItrDetailsResponse>
                {
                    VerificationCode = VerificationCodes.ItrDetails,

                    Endpoint = ApiEndpoints.CENTER_SINGLE_ITR_DETAILS,

                    LogMessage = "[SprintProvider] ITR details request mapped: {ClientId}",

                    LogValueSelector = providerRequest => providerRequest.client_id
                },

                [VerificationCodes.ItrList] =
                new VerificationDefinition<
                    ItrListRequest,
                    ItrListDto,
                    ItrListResponse,
                    PS_ItrListResponse>
                {
                    VerificationCode = VerificationCodes.ItrList,

                    Endpoint = ApiEndpoints.CENTER_ITR_LIST,

                    LogMessage = "[SprintProvider] ITR list request mapped: {ClientId}",

                    LogValueSelector = providerRequest => providerRequest.client_id
                },

                [VerificationCodes.ProfileGet] =
                new VerificationDefinition<
                    ProfileGetRequest,
                    ProfileGetDto,
                    ProfileGetResponse,
                    PS_ProfileGetResponse>
                {
                    VerificationCode = VerificationCodes.ProfileGet,

                    Endpoint = ApiEndpoints.CENTER_PROFILE_GET,

                    LogMessage = "[SprintProvider] Profile get request mapped: {ClientId}",

                    LogValueSelector = providerRequest => providerRequest.client_id
                },

                [VerificationCodes.ItrSubmitOtp] =
                new VerificationDefinition<
                    ItrSubmitOtpRequest,
                    ItrSubmitOtpDto,
                    ItrSubmitOtpResponse,
                    PS_ItrSubmitOtpResponse>
                {
                    VerificationCode = VerificationCodes.ItrSubmitOtp,

                    Endpoint = ApiEndpoints.CENTER_ITR_SUBMIT_OTP,

                    LogMessage = "[SprintProvider] ITR submit OTP request mapped: {ClientId}",

                    LogValueSelector = providerRequest => providerRequest.client_id
                },

                [VerificationCodes.ItrForgetPassword] =
                new VerificationDefinition<
                    ItrForgetPasswordRequest,
                    ItrForgetPasswordDto,
                    ItrForgetPasswordResponse,
                    PS_ItrForgetPasswordResponse>
                {
                    VerificationCode = VerificationCodes.ItrForgetPassword,

                    Endpoint = ApiEndpoints.CENTER_ITR_FORGET_PASSWORD,

                    LogMessage = "[SprintProvider] ITR forget password request mapped: {PanNumber}",

                    LogValueSelector = providerRequest => providerRequest.password
                },

                [VerificationCodes.ItrCreateClient] =
                new VerificationDefinition<
                    ItrCreateClientRequest,
                    ItrCreateClientDto,
                    ItrCreateClientResponse,
                    PS_ItrCreateClientResponse>
                {
                    VerificationCode = VerificationCodes.ItrCreateClient,

                    Endpoint = ApiEndpoints.CENTER_ITR_CREATE_CLIENT,

                    LogMessage = "[SprintProvider] ITR create client request mapped: {UserName}",

                    LogValueSelector = providerRequest => providerRequest.username
                },

                [VerificationCodes.UploadStatement] =
                new VerificationDefinition<
                    BankStatementUploadRequest,
                    BankStatementUploadRequestDto,
                    BankStatementUploadRequest,
                    PS_BankStatementUploadResponse>
                {
                    VerificationCode = VerificationCodes.UploadStatement,

                    Endpoint = ApiEndpoints.CENTER_UPLOAD_STATEMENT,

                    LogMessage = "[SprintProvider] Upload statement request mapped: {File}",

                    LogValueSelector = providerRequest => providerRequest.file
                },

                [VerificationCodes.ReportFetch] =
                new VerificationDefinition<
                    ReportFetchRequest,
                    ReportFetchRequestDto,
                    ReportFetchResponse,
                    PS_ReportFetchResponse>
                {
                    VerificationCode = VerificationCodes.ReportFetch,

                    Endpoint = ApiEndpoints.CENTER_REPORT_FETCH,

                    LogMessage = "[SprintProvider] Report fetch request mapped: {ReportId}",

                    LogValueSelector = providerRequest => providerRequest.transaction_id
                },

                [VerificationCodes.BulkUploadGst] =
                new VerificationDefinition<
                    BulkUploadGstRequest,
                    BulkUploadGstItemDto,
                    BulkUploadGstResponse,
                    PS_BulkUploadGstResponse>
                {
                    VerificationCode = VerificationCodes.BulkUploadGst,

                    Endpoint = ApiEndpoints.CENTER_GST_VALIDATE,

                    LogMessage = "[SprintProvider] Bulk GST upload request mapped: {Id}",

                    LogValueSelector = providerRequest => providerRequest.id_number
                },

                [VerificationCodes.BulkUploadRc] =
                new VerificationDefinition<
                    BulkUploadRCRequest,
                    BulkUploadRcItemDto,
                    BulkUploadRCResponse,
                    PS_BulkUploadRCResponse>
                {
                    VerificationCode = VerificationCodes.BulkUploadRc,

                    Endpoint = ApiEndpoints.CENTER_RC_VERIFY,

                    LogMessage = "[SprintProvider] Bulk RC upload request mapped: {Id}",

                    LogValueSelector = providerRequest => providerRequest.id_number
                },

                [VerificationCodes.OcrValidate] =
                new VerificationDefinition<
                    OCRUploadRequest,
                    OCRUploadDto,
                    OCRUploadResponse,
                    PS_OCRUploadResponse>
                {
                    VerificationCode = VerificationCodes.OcrValidate,

                    Endpoint = ApiEndpoints.CENTER_OCR_UPLOAD,

                    LogMessage = "[SprintProvider] OCR validate request mapped.",

                    LogValueSelector = static _ => "OCR_VALIDATE"
                },

                [VerificationCodes.PaisaDrop] =
                new VerificationDefinition<
                    PaisaDropRequest,
                    PaisaDropRequestDto,
                    PaisaDropResponse,
                    PS_PaisaDropResponse>
                {
                    VerificationCode = VerificationCodes.PaisaDrop,

                    Endpoint = ApiEndpoints.CENTER_PROCESS_DROP,

                    LogMessage = "[SprintProvider] PaisaDrop request mapped: {AccountNumber}",

                    LogValueSelector = providerRequest => providerRequest.account_number
                },

                [VerificationCodes.UanBasicV2] =
                new VerificationDefinition<
                    UanBasicRequest,
                    UanBasicV2Dto,
                    UanBasicResponse,
                    PS_UanBasicResponse>
                {
                    VerificationCode = VerificationCodes.UanBasicV2,

                    Endpoint = ApiEndpoints.CENTER_UAN_BASIC_V2,

                    LogMessage = "[SprintProvider] UAN Basic V2 request mapped: {UanNumber}",

                    LogValueSelector = providerRequest => providerRequest.uan
                },

                [VerificationCodes.EpfoPassbookDownload] =
                new VerificationDefinition<
                    EpfoPassbookRequest,
                    EPFO_PassbookDto,
                    EpfoPassbookResponse,
                    PS_EpfoPassbookResponse>
                {
                    VerificationCode = VerificationCodes.EpfoPassbookDownload,

                    Endpoint = ApiEndpoints.CENTER_PASSBOOK_DOWNLOAD,

                    LogMessage = "[SprintProvider] EPFO passbook download request mapped: {ClientId}",

                    LogValueSelector = providerRequest => providerRequest.client_id
                },

                [VerificationCodes.EpfoKycFetch] =
                new VerificationDefinition<
                    EpfoPassbookRequest,
                    EPFO_PassbookDto,
                    EpfoPassbookResponse,
                    PS_EpfoPassbookResponse>
                {
                    VerificationCode = VerificationCodes.EpfoKycFetch,

                    Endpoint = ApiEndpoints.CENTER_KYC_DETAILS,

                    LogMessage = "[SprintProvider] EPFO KYC fetch request mapped: {ClientId}",

                    LogValueSelector = providerRequest => providerRequest.client_id
                },

                [VerificationCodes.EpfoWithoutOtp] =
                new VerificationDefinition<
                    EPFORequest,
                    EPFODto,
                    EPFOResponse,
                    PS_EPFOResponse>
                {
                    VerificationCode = VerificationCodes.EpfoWithoutOtp,

                    Endpoint = ApiEndpoints.CENTER_EPFO_WITHOUT_OTP,

                    LogMessage = "[SprintProvider] EPFO without OTP request mapped: {UanNumber}",

                    LogValueSelector = providerRequest => providerRequest.uan_number
                },

                [VerificationCodes.MobileImeiVerification] =
                new VerificationDefinition<
                    MobileImeiVerificationRequest,
                    MobileImeiVerificationDto,
                    MobileImeiVerificationResponse,
                    PS_MobileImeiVerificationResponse>
                {
                    VerificationCode = VerificationCodes.MobileImeiVerification,

                    Endpoint = ApiEndpoints.CENTER_MOBILE_IMEI_VERIFICATION,

                    LogMessage = "[SprintProvider] IMEI verification request mapped: {ImeiNumber}",

                    LogValueSelector = providerRequest => providerRequest.imei_no
                },

                [VerificationCodes.CreditReportCheckerEx] =
                new VerificationDefinition<
                    CreditReportCheckExRequest,
                    CreditReportCheckExRequestDto,
                    CreditReportCheckExResponse,
                    PS_CreditReportCheckExResponse>
                {
                    VerificationCode = VerificationCodes.CreditReportCheckerEx,

                    Endpoint = ApiEndpoints.CENTER_CREDIT_REPORT_CHECK_EX,

                    LogMessage = "[SprintProvider] Credit report checker request mapped: {mobile}",

                    LogValueSelector = providerRequest => providerRequest.Mobile
                },

                [VerificationCodes.CreditReportFetchEq] =
                new VerificationDefinition<
                    CreditReportFetchEQRequest,
                    CreditReportFetchEQRequestDto,
                    CreditReportCheckExResponse,
                    PS_CreditReportFetchEQResponse>
                {
                    VerificationCode = VerificationCodes.CreditReportFetchEq,

                    Endpoint = ApiEndpoints.CENTER_CREDIT_REPORT_FETCH_EQ,

                    LogMessage = "[SprintProvider] Credit report fetch request mapped: {mobile}",

                    LogValueSelector = providerRequest => providerRequest.mobile
                },

                [VerificationCodes.ShopEstablishmentDetails] =
                new VerificationDefinition<
                    shopestablishmentRequest,
                    shopestablishmentDto,
                    shopestablishmentResponse,
                    PS_ShopestablishmentResponse>
                {
                    VerificationCode = VerificationCodes.ShopEstablishmentDetails,

                    Endpoint = ApiEndpoints.CENTER_SHOP_ESTABLISHMENT_DETAILS,

                    LogMessage = "[SprintProvider] Shop establishment details request mapped: {ShopId}",

                    LogValueSelector = providerRequest => providerRequest.shop_number
                },

                [VerificationCodes.ReverseRc] =
                new VerificationDefinition<
                    RcReverseRequest,
                    RCReverseDto,
                    RcReverseResponse,
                    PS_RcReverseResponse>
                {
                    VerificationCode = VerificationCodes.ReverseRc,

                    Endpoint = ApiEndpoints.CENTER_RC_REVERSE_VERIFICATION,

                    LogMessage = "[SprintProvider] Reverse RC request mapped: {ChassisNumber}",

                    LogValueSelector = providerRequest => providerRequest.chassis_number
                }
            };
    }
}
