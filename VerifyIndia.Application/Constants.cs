using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application
{
    public static class Constants
    {
        public static readonly string PlatformTenantIdentifier = "authenticateindia";
        public static class EmailTemplateConstants
        {
            /// <summary>
            /// Master_EmailTemplate Id for "Reset Your Password" email
            /// </summary>
            public const decimal ResetPassword = 3;
            public const decimal AccountCredential = 2;
            public const decimal WLResetPassword = 5;
            public const decimal ProfileUpdateOTPVerification = 8;

            // You can add other templates here
            // public const int WelcomeEmail = 12;
            // public const int OTPVerification = 13;
        }

        public static class VerificationConstants
        {
            public const string EmailOtp = "EMAIL_OTP_";
            public const string EmailUpdate = "EMAIL_UPDATE_";

            public const string MobileOtp = "MOBILE_OTP_";
            public const string MobileUpdate = "MOBILE_UPDATE_";
        }
        public static class ClaimConstants
        {
            public const string UUID = "UUID";
            public const string RoleUUID = "RoleUUID";
            public const string IP = "IP";
            public const string ProfilePic = "ProfilePic";
            public const string EmployeeCode = "EmployeeCode";
            public const string Honorific = "Honorific";
            public const string LastLogin = "LastLogin";
        }
        public static class GroupNameConstants
        {
            public const string AIX = "aix";
            public const string WhiteLabelAdmin = "whitelabel-admin";
            public const string Sandbox = "sandbox";
            public const string SprintVerify = "sprint";
            public const string Common = "common";
            public const string CustomerAuthentication = "customer-authentication";
            public const string CustomerDashboard = "customer-dashboard";
            public const string CustomerSupportTicket = "customer-SupportTicket";
            public const string CustomerRegistration = "customer-registration";
            public const string CustomerPanel = "customer-panel";
            public const string CustomerCart = "customer-cart";
            public const string CustomerTransaction = "customer-transaction";
            public const string CustomerConsent = "customer-consent";
            public const string Notification = "notification";
        }

        public static class NextActions
        {
            public const string SELECT_ACCOUNT_TYPE = "SELECT_ACCOUNT_TYPE";
            public const string AADHAAR_Verification = "AADHAAR_Verification";
            public const string BASIC_INFO = "BASIC_INFO";
            public const string VIDEO_KYC = "VIDEO_KYC";
            public const string TERMS = "TERMS";
            public const string REFERRAL_CODE = "REFERRAL_CODE";
            public const string DOSDONTS = "DOSDONTS";
            public const string DASHBOARD = "DASHBOARD";
        }
        public static class PolicyCodes
        {
            public const string RegLegalAgreement = "RegistrationLegalAgreement";
            public const string RegKyc = "RegistrationKyc";
            public const string RegFraudDeclaration = "RegistrationFraudDeclaration";
            public const string KycRecording = "KYC_RECORDING";
            public const string IdentityAndFraud = "IDENTITY_AND_FRAUD";
            public const string PrivacyPolicy = "PRIVACY_POLICY";
            public const string TermsOfUse = "TERMS_OF_USE";
            public const string CONSENT_NOTICE = "CONSENT_NOTICE";
        }
        public static class NomenclatureModuleKeys
        {
            public const string Employee = "Employee";
            public const string Agent = "Agent";
            public const string SupportTicket = "SupportTicket";
        }

        public static class DataTypeConstants
        {
            public const string String = "string";
            public const string Integer = "integer";
            public const string Number = "number";
            public const string Boolean = "boolean";
            public const string Object = "object";
            public const string Array = "array";
            public const string Date = "date";
            public const string DateTime = "datetime";

            public static readonly IReadOnlyList<string> All = new[]
            {
                String,
                Integer,
                Number,
                Boolean,
                Object,
                Array,
                Date,
                DateTime
            };
        }

        //public static class RequestType
        //{
        //    public const string Multipart = "multipart/form-date";
        //    public const string Object = "application/json";
        //}

        public static class FAQ
        {
            public const string FaqSubCategory = "FaqSubcategory";
        }

        public static class CustomerPanel
        {
            public const string CustomerUUID = "customerUUID";
        }

        public static class CustomerTypes
        {
            public const string Customer = "customer";
            public const string Agent = "agent";
            public const string AgentHead = "agenthead";
        }

        public static class VerificationCodes
        {
            public const string Pan = "PAN";
            public const string AadhaarSendOTP = "AADHAAR_SEND_OTP";
            public const string AadhaarVerifyOtp = "AADHAAR_VERIFY_OTP";
            public const string Dl = "DL";
            public const string Voter = "VOTER";
            public const string Gst = "GST";
            public const string Passport = "PASSPORT";
            public const string Tan = "TAN";
            public const string Rc = "RC";
            public const string Iec = "IEC";
            public const string Fssai = "FSSAI";
            public const string Din = "DIN";
            public const string IfscLookup = "IFSC_LOOKUP";
            public const string PincodeInfo = "PINCODE_INFO";
            public const string IpLookup = "IP_LOOKUP";
            public const string MobileOperatorCheck = "MOBILE_OPERATOR_CHECK";
            public const string EmailChecker = "EMAIL_CHECKER";
            public const string VpnProxyCheck = "VPN_PROXY_CHECK";
            public const string CardValidation = "CARD_VALIDATION";
            public const string PanToGst = "PAN_TO_GST";
            public const string CompanyToTan = "COMPANY_TO_TAN";
            public const string DinToMobileNumber = "DIN_TO_MOBILE_NUMBER";
            public const string MobileToUpiVpa = "MOBILE_TO_UPI_VPA";
            public const string UanBasic = "UAN_BASIC";
            public const string LeiVerification = "LEI_VERIFICATION";
            public const string RtoInformation = "RTO_INFORMATION";
            public const string StockPrice = "STOCK_PRICE";
            public const string ReverseGeolocation = "REVERSE_GEOLOCATION";
            public const string FuelPrice = "FUEL_PRICE";
            public const string NameMatch = "NAME_MATCH";
            public const string AadhaarQrCheck = "AADHAAR_QR_CHECK";
            public const string UpiVerification = "UPI_VERIFICATION";
            public const string FaceMatch = "FACE_MATCH";
            public const string LivenessCheck = "LIVENESS_CHECK";
            public const string GstAdvance = "GST_ADVANCE";
            public const string TanAdvance = "TAN_ADVANCE";
            public const string UdyamAadhaarV2 = "UDYAM_AADHAAR";
            public const string RcAdvance = "RC_ADVANCE";
            public const string VehicleChallanV1 = "VEHICLE_CHALLAN_V1";
            public const string VehicleChallanV2 = "VEHICLE_CHALLAN";
            public const string McaVerify = "MCA_VERIFY";
            public const string ItrComplianceCheck = "ITR_COMPLIANCE_CHECK";
            public const string GetBalance = "GET_BALANCE";
            public const string CheckStatus = "CHECK_STATUS";
            public const string CompanyNameToCin = "COMPANY_NAME_TO_CIN";
            public const string MobileIntelligence = "MOBILE_INTELLIGENCE";
            public const string StateList = "STATE_LIST";
            public const string CreditReportStateList = "CREDIT_REPORT_STATE_LIST";
            public const string TelecomSendOtp = "TELECOM_SEND_OTP";
            public const string TelecomVerifyOtp = "TELECOM_VERIFY_OTP";
            public const string SendOtp = "SEND_OTP";
            public const string VerifyOtp = "VERIFY_OTP";
            public const string CrimeCheckIndividual = "CRIME_CHECK_INDIVIDUAL";
            public const string CrimeCheckCompany = "CRIME_CHECK_COMPANY";
            public const string CrimeCheckDownloadPdfReport = "CRIME_CHECK_DOWNLOAD_PDF_REPORT";
            public const string CrimeCheckDownloadJsonReport = "CRIME_CHECK_DOWNLOAD_JSON_REPORT";
            public const string CourtCaseStatus = "COURT_CASE_STATUS";
            public const string BavHybridV1 = "BAV_HYBRID_V1";
            public const string BavHybridV2 = "BAV_HYBRID_V2";
            public const string BavHybridV3 = "BAV_HYBRID";
            public const string BavPennylessV1 = "BAV_PENNYLESS_V1";
            public const string BavPennylessV2 = "BAV_PENNYLESS_V2";
            public const string BavPennylessV3 = "BAV_PENNYLESS";
            public const string BavPennydropV1 = "BAV_PENNYDROP_V1";
            public const string BavPennydropV2 = "BAV_PENNYDROP";
            public const string DigilockerInitiateSession = "DIGILOCKER_INITIATE_SESSION";
            public const string DigilockerAccessToken = "DIGILOCKER_ACCESS_TOKEN";
            public const string DigilockerIssuedFiles = "DIGILOCKER_ISSUED_FILES";
            public const string DigilockerDownloadPdf = "DIGILOCKER_DOWNLOAD_PDF";
            public const string DigilockerDownloadXml = "DIGILOCKER_DOWNLOAD_XML";
            public const string DigilockerEaadhaar = "DIGILOCKER_EAADHAAR";
            public const string TwentySixAsDetails = "TWENTY_SIX_AS_DETAILS";
            public const string TwentySixAsList = "TWENTY_SIX_AS_LIST";
            public const string ItrDetails = "ITR_DETAILS";
            public const string ItrList = "ITR_LIST";
            public const string ProfileGet = "PROFILE_GET";
            public const string ItrSubmitOtp = "ITR_SUBMIT_OTP";
            public const string ItrForgetPassword = "ITR_FORGET_PASSWORD";
            public const string ItrCreateClient = "ITR_CREATE_CLIENT";
            public const string UploadStatement = "UPLOAD_STATEMENT";
            public const string ReportFetch = "REPORT_FETCH";
            public const string BulkUploadGst = "BULK_UPLOAD_GST";
            public const string BulkUploadRc = "BULK_UPLOAD_RC";
            public const string BankAccountVerifyBulk = "BANK_ACCOUNT_VERIFY_BULK";
            public const string OcrValidate = "OCR_VALIDATE";
            public const string PaisaDrop = "PAISA_DROP";
            public const string UanBasicV2 = "UAN_BASIC";
            public const string EpfoPassbookDownload = "EPFO_PASSBOOK_DOWNLOAD";
            public const string EpfoKycFetch = "EPFO_KYC_FETCH";
            public const string EpfoWithoutOtp = "EPFO_WITHOUT_OTP";
            public const string MobileImeiVerification = "MOBILE_IMEI_VERIFICATION";
            public const string DlV2 = "DRIVING_LICENSE";
            public const string CreditReportCheckerEx = "CREDIT_REPORT_CHECKER_EX";
            public const string CreditReportFetchEq = "CREDIT_REPORT_FETCH_EQ";
            public const string StateListDetails = "STATE_LIST_DETAILS";
            public const string ShopEstablishmentDetails = "SHOP_ESTABLISHMENT_DETAILS";
            public const string ReverseRc = "REVERSE_RC";
            public const string EmailCheckerV2 = "EMAIL_CHECKER_V2";
            public const string PanAdharLink = "PAN_ADHAR_LINK";
            public const string SearchGst = "SEARCH_GST";

        }
        public static class SecretMaskConstants
        {
            public const string MaskedSecretValue = "********";
        }
        public static class PaymentGateways
        {
            public const string RazorPay = "RAZOR_PAY";
        }
    }
}
