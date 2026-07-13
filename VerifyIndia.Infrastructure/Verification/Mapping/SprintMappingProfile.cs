using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Upgrow.Application.DTO.Verification.Common.Request;
using Upgrow.Application.DTO.Verification.Common.Response;
using Upgrow.Application.DTO.Verification.SprintVerify;
using Upgrow.Application.DTO.Verification.SprintVerify.Response;

namespace Upgrow.Infrastructure.Verification.Mapping
{
    public class SprintMappingProfile : Profile
    {
        public SprintMappingProfile()
        {
            #region Pan Verification
            // PAN: Internal → Sprint (Request)
            CreateMap<PanVerifyRequest, PanVerifyRequestDto>()
                .ForMember(d => d.pannumber, opt => opt.MapFrom(s => s.pan));

            // PAN: Sprint → Internal (Response)
            CreateMap<SP_PanVerifyResponse, PanVerifyResponse>()
            .ForMember(d => d.gender, opt => opt.MapFrom(s => s.gender))
            .ForMember(d => d.full_name, opt => opt.MapFrom(s => s.full_name))
            .ForMember(d => d.category, opt => opt.MapFrom(s => s.category))
            .ForMember(d => d.pan_number, opt => opt.MapFrom(s => s.pan_number));
            #endregion

            #region Driving License v2
            CreateMap<DrivingLicenceRequest, DrivingLicenceV1Dto>()
              .ForMember(d => d.id_number, opt => opt.MapFrom(s => s.id_number))
              .ForMember(d => d.dob, opt => opt.MapFrom(s => s.dob))
              .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid));

            CreateMap<PS_DrivingLicenceResponse, DrivingLicenceResponse>();
            #endregion

            #region OTP Send

            CreateMap<OtpSendRequest, OTPSendDto>()
                .ForMember(d => d.epfo_number, opt => opt.MapFrom(s => s.epfo_number));

            CreateMap<PS_OtpSendResponse, OtpSendResponse>();

            #endregion

            #region OTP Verify

            CreateMap<OTPVerifyRequest, OTPVerifyDto>()
                .ForMember(d => d.client_id, opt => opt.MapFrom(s => s.client_id))
                .ForMember(d => d.otp, opt => opt.MapFrom(s => s.otp));

            CreateMap<PS_OTPVerifyResponse, OTPVerifyResponse>();


            #endregion

            #region BAVHybridV3
            CreateMap<BAVHybridRequest, BAVHybridV3>()
                .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
                .ForMember(d => d.account_number, opt => opt.MapFrom(s => s.account_number))
                .ForMember(d => d.ifsc_code, opt => opt.MapFrom(s => s.ifsc_code))
                .ForMember(d => d.txn_type, opt => opt.MapFrom(s => s.txn_type));

            CreateMap<PS_BAVHybridResponse, BAVHybridResponse>();

            #endregion

            #region PennyDropV2
            CreateMap<PennyDropVerifyRequest, BankAccountVerification>()
                .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
                .ForMember(d => d.ifsc_code, opt => opt.MapFrom(s => s.ifsc))
                .ForMember(d => d.account_number, opt => opt.MapFrom(s => s.account_number));

            CreateMap<PS_PennyDropVerifyResponse, PennyDropVerifyResponse>();
            CreateMap<PennyDropDetails, PennyDropRequestDetails>();
            #endregion

            #region PennyLess V3
            CreateMap<PennyLessVerifyRequest, BankAccountVerification>()
                .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
                .ForMember(d => d.ifsc_code, opt => opt.MapFrom(s => s.ifsc_code))
                .ForMember(d => d.account_number, opt => opt.MapFrom(s => s.account_number));

            CreateMap<PS_PennyLessVerifyResponse, PennyLessVerifyResponse>()
                .ForMember(d => d.beneficiary_ifsc, opt => opt.MapFrom(s => s.beneficiary_ifsc))
                .ForMember(d => d.bank_name, opt => opt.MapFrom(s => s.bank_name))
                .ForMember(d => d.beneficiary_name, opt => opt.MapFrom(s => s.bank_name))
                .ForMember(d => d.branch_name, opt => opt.MapFrom(s => s.beneficiary_name))
                .ForMember(d => d.beneficiary_account, opt => opt.MapFrom(s => s.beneficiary_account));
            #endregion

            #region UanBasic
            CreateMap<UanBasicRequest, UanBasicV2Dto>()
                .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
                .ForMember(d => d.type, opt => opt.MapFrom(s => s.type))
                .ForMember(d => d.method, opt => opt.MapFrom(s => s.method))
                .ForMember(d => d.employee_name, opt => opt.MapFrom(s => s.employee_name))
                .ForMember(d => d.employer_name, opt => opt.MapFrom(s => s.employer_name))
                .ForMember(d => d.mobile, opt => opt.MapFrom(s => s.mobile))
                .ForMember(d => d.dob, opt => opt.MapFrom(s => s.dob))
                .ForMember(d => d.uan, opt => opt.MapFrom(s => s.uan))
                .ForMember(d => d.pan_number, opt => opt.MapFrom(s => s.pan_number))
                .ForMember(d => d.dob, opt => opt.MapFrom(s => s.dob));

            CreateMap<PS_UanBasicResponse, UanBasicResponse>();
            #endregion

            #region UdayamAadhaarV2
            CreateMap<UdyamAadhaarRequest, UdyamAadhaarV1Dto>()
               .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
               .ForMember(d => d.id_number, opt => opt.MapFrom(s => s.id_number));

            CreateMap<PS_UdyamAadhaarResponse, UdyamAadhaarResponse>();
            CreateMap<MainDetails1, MainDetails>();

            CreateMap<EnterpriseType1, EnterpriseType>();

            CreateMap<LocationOfPlantDetail1, LocationOfPlantDetail>();

            CreateMap<NicCode1, NicCode>();

            #endregion

            #region  VehicleChallanV2
            CreateMap<VehicalChallanRequest, VehicalChallanV2Dto>()
              .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
              .ForMember(d => d.rc_number, opt => opt.MapFrom(s => s.rc_number))
              .ForMember(d => d.chassis_number, opt => opt.MapFrom(s => s.chassis_number))
              .ForMember(d => d.engine_number, opt => opt.MapFrom(s => s.engine_number));

            CreateMap<PS_VehicalChallanResponse, VehicalChallanResponse>();
            #endregion

            #region ifsc Verification
            // IFSC: Internal → Sprint (Request)
            CreateMap<IfscVerifyRequest, IFSCLookupRequestDto>()
                .ForMember(d => d.ifsc, opt => opt.MapFrom(s => s.ifsc))
                .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid));

            // IFSC: Internal → Sprint (Response)
            CreateMap<PS_IfscVerifyResponse, IfscVerifyResponse>()
                .ForMember(d => d.ifsc, opt => opt.MapFrom(s => s.ifsc))
                .ForMember(d => d.name, opt => opt.MapFrom(s => s.name))
                .ForMember(d => d.code, opt => opt.MapFrom(s => s.code))
                .ForMember(d => d.branch, opt => opt.MapFrom(s => s.branch))
                .ForMember(d => d.micr, opt => opt.MapFrom(s => s.micr))
                .ForMember(d => d.address, opt => opt.MapFrom(s => s.address))
                .ForMember(d => d.city, opt => opt.MapFrom(s => s.city))
                .ForMember(d => d.state, opt => opt.MapFrom(s => s.state))
                .ForMember(d => d.district, opt => opt.MapFrom(s => s.district))
                .ForMember(d => d.contact, opt => opt.MapFrom(s => s.contact))
                .ForMember(d => d.upi, opt => opt.MapFrom(s => s.upi))
                .ForMember(d => d.imps, opt => opt.MapFrom(s => s.imps))
                .ForMember(d => d.neft, opt => opt.MapFrom(s => s.neft))
                .ForMember(d => d.rtgs, opt => opt.MapFrom(s => s.rtgs))
                .ForMember(d => d.swift, opt => opt.MapFrom(s => s.swift))
                .ForMember(d => d.logo, opt => opt.MapFrom(s => s.logo));

            #endregion

            #region Aadhar QR Check
            CreateMap<AadharQRCheckRequest, AdhaarQRCheckDto>()
               .ForMember(d => d.aadhaar_image, opt => opt.MapFrom(s => s.aadhaar_image));
            CreateMap<PS_AadharQRCheckResponse, AadharQRCheckResponse>();
            #endregion

            #region Bank Statement Upload
            CreateMap<BankStatementUploadRequest, BankStatementUploadRequestDto>()
               .ForMember(d => d.file, opt => opt.MapFrom(s => s.file))
               .ForMember(d => d.reqid, opt => opt.MapFrom(s => s.reqid));
            CreateMap<PS_BankStatementUploadResponse, BankStatementUploadResponse>();
            #endregion

            #region ReportFetch
            CreateMap<ReportFetchRequest, ReportFetchRequestDto>()
               .ForMember(d => d.transaction_id, opt => opt.MapFrom(s => s.transaction_id));
            CreateMap<PS_ReportFetchResponse, ReportFetchResponse>();
            #endregion

            #region Bulk Gst Upload
            CreateMap<BulkUploadGstRequest, BulkUploadGstItemDto>()
               .ForMember(d => d.id_number, opt => opt.MapFrom(s => s.id_number))
               .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid));
            CreateMap<PS_BulkUploadGstResponse, BulkUploadGstResponse>();
            #endregion

            #region Bulk RC Upload
            CreateMap<BulkUploadRCRequest, BulkUploadRcItemDto>()
               .ForMember(d => d.id_number, opt => opt.MapFrom(s => s.id_number))
               .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid));
            CreateMap<PS_BulkUploadRCResponse, BulkUploadRCResponse>();
            #endregion

            #region Card validation
            CreateMap<CardValidationRequest, CardValidationDto>()
               .ForMember(d => d.card_num, opt => opt.MapFrom(s => s.card_num));
            CreateMap<PS_CardValidationResponse, CardValidationResponse>();
            #endregion

            #region STATUS CHECK API
            CreateMap<CommonStatusCheckRequest, CommonStatusCheckRequestDto>()
               .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid));
            CreateMap<PS_CommonStatusCheckResponse, CommonStatusCheckResponse>();
            #endregion

            #region CompanyName to CIN
            CreateMap<CompanyNameToCinRequest, CompanyNameToCinRequestDto>()
             .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
             .ForMember(d => d.CompanyName, opt => opt.MapFrom(s => s.CompanyName));
            CreateMap<PS_CompanyNameToCinResponse, CompanyNameToCinResponse>();
            #endregion

            #region Court Case Status
            CreateMap<CourtCaseStatusRequest, CourtCaseStatusDto>()
             .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
             .ForMember(d => d.name, opt => opt.MapFrom(s => s.name))
             .ForMember(d => d.father_name, opt => opt.MapFrom(s => s.father_name))
             .ForMember(d => d.address, opt => opt.MapFrom(s => s.address))
             .ForMember(d => d.state_name, opt => opt.MapFrom(s => s.state_name))
             .ForMember(d => d.year, opt => opt.MapFrom(s => s.year))
             .ForMember(d => d.source, opt => opt.MapFrom(s => s.source))
             .ForMember(d => d.case_type, opt => opt.MapFrom(s => s.case_type))
             .ForMember(d => d.search_type, opt => opt.MapFrom(s => s.search_type))
             .ForMember(d => d.filters, opt => opt.MapFrom(s => s.filters))
             .ForMember(d => d.category, opt => opt.MapFrom(s => s.category));
            CreateMap<PS_CourtCaseStatusResponse, CourtCaseStatusResponse>();
            #endregion

            #region Credit Report Check Ex
            CreateMap<CreditReportCheckExRequest, CreditReportCheckExRequestDto>()
            .ForMember(d => d.Mobile, opt => opt.MapFrom(s => s.Mobile))
            .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
            .ForMember(d => d.Pan, opt => opt.MapFrom(s => s.Pan));
            CreateMap<PS_CreditReportCheckExResponse, CreditReportCheckExResponse>();
            #endregion

            #region Credit Report Check Eq
            CreateMap<CreditReportFetchEQRequest, CreditReportFetchEQRequestDto>()
            .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
            .ForMember(d => d.name, opt => opt.MapFrom(s => s.name))
            .ForMember(d => d.mobile, opt => opt.MapFrom(s => s.mobile))
            .ForMember(d => d.document_id, opt => opt.MapFrom(s => s.document_id))
            .ForMember(d => d.dateofbirth, opt => opt.MapFrom(s => s.dateofbirth))
            .ForMember(d => d.address, opt => opt.MapFrom(s => s.address))
            .ForMember(d => d.pincode, opt => opt.MapFrom(s => s.pincode));
            CreateMap<PS_CreditReportFetchEQResponse, CreditReportCheckExResponse>();
            #endregion

            #region Crime Check Individual
            CreateMap<CrimeCheckIndividualRequest, CrimeCheckIndividualRequestDto>()
            .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
            .ForMember(d => d.FatherName, opt => opt.MapFrom(s => s.FatherName))
            .ForMember(d => d.Address, opt => opt.MapFrom(s => s.Address))
            .ForMember(d => d.Dob, opt => opt.MapFrom(s => s.Dob))
            .ForMember(d => d.PanNumber, opt => opt.MapFrom(s => s.PanNumber))
            .ForMember(d => d.req_tag, opt => opt.MapFrom(s => s.req_tag))
            .ForMember(d => d.TicketSize, opt => opt.MapFrom(s => s.TicketSize))
            .ForMember(d => d.Crimewatch, opt => opt.MapFrom(s => s.Crimewatch))
            .ForMember(d => d.ReportMode, opt => opt.MapFrom(s => s.ReportMode));
            CreateMap<PS_CrimeCheckIndividualResponse, CrimeCheckIndividualResponse>();
            #endregion

            #region Crime Check Company
            CreateMap<CrimeCheckCompanyRequest, CrimeCheckCompanyRequestDto>()
            .ForMember(d => d.CompanyName, opt => opt.MapFrom(s => s.CompanyName))
            .ForMember(d => d.CompanyType, opt => opt.MapFrom(s => s.CompanyType))
            .ForMember(d => d.CompanyAddress, opt => opt.MapFrom(s => s.CompanyAddress))
            .ForMember(d => d.Directors, opt => opt.MapFrom(s => s.Directors))
            .ForMember(d => d.ReportMode, opt => opt.MapFrom(s => s.ReportMode))
            .ForMember(d => d.CinNumber, opt => opt.MapFrom(s => s.CinNumber))
            .ForMember(d => d.GstNumber, opt => opt.MapFrom(s => s.GstNumber))
            .ForMember(d => d.req_tag, opt => opt.MapFrom(s => s.req_tag))
            .ForMember(d => d.TicketSize, opt => opt.MapFrom(s => s.TicketSize))
            .ForMember(d => d.CrimeWatch, opt => opt.MapFrom(s => s.CrimeWatch));
            CreateMap<PS_CrimeCheckCompanyResponse, CrimeCheckCompanyResponse>();
            #endregion

            #region CrimeCheckDownloadPdfReport
            CreateMap<CrimeCheckDownloadPdfReportRequest, CrimeCheckDownloadPdfReportRequestDto>()
               .ForMember(d => d.RequestId, opt => opt.MapFrom(s => s.RequestId));
            CreateMap<PS_CrimeCheckDownloadPdfReportResponse, CrimeCheckDownloadPdfReportResponse>();
            #endregion

            #region CrimeCheckDownloadJsonReport
            CreateMap<CrimeCheckDownloadJsonReportRequest, CrimeCheckDownloadJsonReportRequestDto>()
               .ForMember(d => d.RequestId, opt => opt.MapFrom(s => s.RequestId));
            CreateMap<PS_CrimeCheckDownloadJsonReportResponse, CrimeCheckDownloadJsonReportResponse>();
            #endregion

            #region DigilockerInitiateSession
            CreateMap<DigilockerInitiateSessionRequest, DigilockerInitiateSessionRequestDto>()
               .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
               .ForMember(d => d.redirecturl, opt => opt.MapFrom(s => s.redirecturl));
            CreateMap<PS_DigilockerInitiateSessionResponse, DigilockerInitiateSessionResponse>();
            #endregion

            #region DigilockerAccessToken
            CreateMap<DigiLockerAccessTokenRequest, DigilockerAccessTokenRequestDto>()
               .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid));
            CreateMap<PS_DigiLockerAccessTokenResponse, DigiLockerAccessTokenResponse>();
            #endregion

            #region DigilockerIssuedFile
            CreateMap<DigilockerIssuedFilesRequest, DigilockerIssuedFilesRequestDto>()
               .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid));
            CreateMap<PS_DigilockerIssuedFilesResponse, DigilockerIssuedFilesResponse>();
            #endregion

            #region DigilockerDownloadPdf
            CreateMap<DigilockerDownloadPdfRequest, DigilockerDownloadPdfRequestDto>()
               .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
               .ForMember(d => d.Uri, opt => opt.MapFrom(s => s.Uri));
            CreateMap<PS_DigilockerDownloadPdfResponse, DigilockerDownloadPdfResponse>();
            #endregion

            #region DigilockerDownloadXml
            CreateMap<DigilockerDownloadXmlRequest, DigilockerDownloadXmlRequestDto>()
               .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
               .ForMember(d => d.Uri, opt => opt.MapFrom(s => s.Uri));
            CreateMap<PS_DigilockerDownloadXmlResponse, DigilockerDownloadXmlResponse>();
            #endregion

            #region DigilockerEaadhaar
            CreateMap<DigilockerEaadhaarRequest, DigilockerEaadhaarRequestDto>()
               .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid));
            CreateMap<PS_DigilockerEaadhaarResponse, DigilockerEaadhaarResponse>();
            #endregion

            #region PaisaDrop
            CreateMap<PaisaDropRequest, PaisaDropRequestDto>()
           .ForMember(d => d.account_number, opt => opt.MapFrom(s => s.account_number))
           .ForMember(d => d.ifsc_code, opt => opt.MapFrom(s => s.ifsc_code));
            CreateMap<PS_PaisaDropResponse, PaisaDropResponse>();
            #endregion

            #region MobileIntelligence
            CreateMap<MobileIntelligenceRequest, MobileIntelligenceRequestDto>()
               .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
               .ForMember(d => d.first_name, opt => opt.MapFrom(s => s.first_name))
               .ForMember(d => d.last_name, opt => opt.MapFrom(s => s.last_name))
               .ForMember(d => d.pan, opt => opt.MapFrom(s => s.pan))
               .ForMember(d => d.phone, opt => opt.MapFrom(s => s.phone));
            CreateMap<PS_MobileIntelligenceResponse, MobileIntelligenceResponse>();
            #endregion

            #region PincodeInfo
            CreateMap<PincodeInfoRequest, PincodeInfoRequestDto>()
           .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
           .ForMember(d => d.pincode, opt => opt.MapFrom(s => s.pincode));
            CreateMap<PS_PincodeInfoResponse, PincodeInfoResponse>()
             .ForMember(d => d.data, opt => opt.MapFrom(s => s.data));
            CreateMap<PincodeData, PincodeInfo>();

            #endregion

            #region VpnProxyCheck
            CreateMap<VpnProxyCheckRequest, VpnProxyCheckRequestDto>()
               .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
               .ForMember(d => d.ip_address, opt => opt.MapFrom(s => s.ip_address));
            CreateMap<PS_VpnProxyCheckResponse, VpnProxyCheckResponse>();
            #endregion

            #region DinVerification
            CreateMap<DinVerificationRequest, DinVerificationRequestDto>()
               .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
               .ForMember(d => d.din_number, opt => opt.MapFrom(s => s.din_number));
            CreateMap<PS_DinVerificationResponse, DinVerificationResponse>();
            #endregion

            #region DinToMobileNumber
            CreateMap<DinToMobileNumberRequest, DinToMobileNumberRequestDto>()
               .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
               .ForMember(d => d.din_number, opt => opt.MapFrom(s => s.din_number));
            CreateMap<PS_DinToMobileNumberResponse, DinToMobileNumberResponse>();
            #endregion

            #region Emailchecker
            CreateMap<EmailCheckerRequest, EmailCheckerDto>()
              .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
              .ForMember(d => d.email, opt => opt.MapFrom(s => s.email));
            CreateMap<PS_EmailCheckerResponse, EmailCheckerResponse>();
            #endregion

            #region EPFOPassbook
            CreateMap<EpfoPassbookRequest, EPFO_PassbookDto>()
              .ForMember(d => d.client_id, opt => opt.MapFrom(s => s.client_id));
            //.ForMember(d => d.Token, opt => opt.MapFrom(s => s.Token))
            //.ForMember(d => d.ContentType, opt => opt.MapFrom(s => s.ContentType))
            //.ForMember(d => d.AuthorisedKey, opt => opt.MapFrom(s => s.AuthorisedKey));
            CreateMap<PS_EpfoPassbookResponse, EpfoPassbookResponse>();
            #endregion

            #region EPFO Without OTP
            CreateMap<EPFORequest, EPFODto>()
             .ForMember(d => d.uan_number, opt => opt.MapFrom(s => s.uan_number));
            CreateMap<PS_EPFOResponse, EPFOResponse>();
            #endregion

            #region Face Match
            CreateMap<FaceMatchRequest, FaceMatchDto>()
               .ForMember(d => d.thereshould, opt => opt.MapFrom(s => s.thereshould))
               .ForMember(d => d.accept, opt => opt.MapFrom(s => s.accept))
               .ForMember(d => d.image1, opt => opt.MapFrom(s => s.image1))
               .ForMember(d => d.image2, opt => opt.MapFrom(s => s.image2))
               .ForMember(d => d.image1_Url, opt => opt.MapFrom(s => s.image1_Url))
               .ForMember(d => d.image2_Url, opt => opt.MapFrom(s => s.image2_Url))
               .ForMember(d => d.accept, opt => opt.MapFrom(s => s.accept));
            CreateMap<PS_FaceMatchResponse, FaceMatchResponse>();
            #endregion

            #region IFSSAI
            CreateMap<FSSAIRequest, FSSAIDto>()
            .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
            .ForMember(d => d.fssai_number, opt => opt.MapFrom(s => s.fssai_number));
            CreateMap<PS_FSSAIResponse, FSSAIResponse>();
            #endregion

            #region FuelPriceFetch
            CreateMap<FuelPriceFetchRequest, FuelPriceFetchDto>()
               .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
               .ForMember(d => d.citystate, opt => opt.MapFrom(s => s.citystate));
            CreateMap<PS_FuelPriceFetchResponse, FuelPriceFetchResponse>();
            #endregion

            #region IEC Verification
            CreateMap<IECVerifyRequest, IECVerifyDto>()
             .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
             .ForMember(d => d.iec_number, opt => opt.MapFrom(s => s.iec_number));
            CreateMap<PS_IECVerifyResponse, IECVerifyResponse>();
            #endregion

            #region IPlookup
            CreateMap<IplookupRequest, IplookupDto>()
               .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
               .ForMember(d => d.ip_address, opt => opt.MapFrom(s => s.ip_address));
            CreateMap<PS_IplookupResponse, IplookupResponse>();
            #endregion

            #region ITRCompliancecheck
            CreateMap<ITRCompliancecheckRequest, ITRCompliancecheckDto>()
               .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
               .ForMember(d => d.pan_number, opt => opt.MapFrom(s => s.pan_number));
            CreateMap<PS_ITRCompliancecheckResponse, ITRCompliancecheckResponse>();
            #endregion

            #region ItrCreateClient
            CreateMap<ItrCreateClientRequest, ItrCreateClientDto>()
               .ForMember(d => d.username, opt => opt.MapFrom(s => s.username))
               .ForMember(d => d.password, opt => opt.MapFrom(s => s.password));
            CreateMap<PS_ItrCreateClientResponse, ItrCreateClientResponse>()
             .ForMember(d => d.client_id, opt => opt.MapFrom(s => s.client_id))
             .AfterMap((src, dest, context) =>
             {
                 if (context.Items.TryGetValue("Request", out var req) &&
                 req is ItrCreateClientRequest request)
                 {
                     dest.request = new ClientData
                     {
                         username = request.username,
                         password = request.password
                     };
                 }
             });
            #endregion

            #region ItrForgetPassword
            CreateMap<ItrForgetPasswordRequest, ItrForgetPasswordDto>()
               .ForMember(d => d.client_id, opt => opt.MapFrom(s => s.client_id))
               .ForMember(d => d.password, opt => opt.MapFrom(s => s.password));
            CreateMap<PS_ItrForgetPasswordResponse, ItrForgetPasswordResponse>();
            #endregion

            #region ItrList
            CreateMap<ItrListRequest, ItrListDto>()
            .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
            .ForMember(d => d.client_id, opt => opt.MapFrom(s => s.client_id));
            CreateMap<PS_ItrListResponse, ItrListResponse>();

            #endregion

            #region ItrSubmitotp
            CreateMap<ItrSubmitOtpRequest, ItrSubmitOtpDto>()
               .ForMember(d => d.client_id, opt => opt.MapFrom(s => s.client_id))
               .ForMember(d => d.otp, opt => opt.MapFrom(s => s.otp));
            CreateMap<PS_ItrSubmitOtpResponse, ItrSubmitOtpResponse>();
            #endregion

            #region LEIVerify
            CreateMap<LEIVerifyRequest, LEIVerifyDto>()
           .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
           .ForMember(d => d.id_number, opt => opt.MapFrom(s => s.id_number));
            //.ForMember(d => d.Token, opt => opt.MapFrom(s => s.Token))
            //.ForMember(d => d.Authorisedkey, opt => opt.MapFrom(s => s.Authorisedkey))
            //.ForMember(d => d.ContentType, opt => opt.MapFrom(s => s.ContentType));
            CreateMap<PS_LEIVerifyResponse, LEIVerifyResponse>();
            #endregion

            #region Liveness
            CreateMap<LivenessRequest, LivenessDto>()
               .ForMember(d => d.video, opt => opt.MapFrom(s => s.video))
               .ForMember(d => d.video_Url, opt => opt.MapFrom(s => s.video_Url));
            CreateMap<PS_LivenessResponse, LivenessResponse>();
            #endregion

            #region MCA
            CreateMap<MCARequest, MCADto>()
            .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
            .ForMember(d => d.id_number, opt => opt.MapFrom(s => s.id_number));
            CreateMap<PS_MCAResponse, MCAResponse>();
            CreateMap<PS_McaDetails, McaDetails>();

            CreateMap<PS_McaCompanyInfo, McaCompanyInfo>();

            CreateMap<PS_McaDirector, McaDirector>();
            #endregion

            #region MobileImeiVerification
            CreateMap<MobileImeiVerificationRequest, MobileImeiVerificationDto>()
           .ForMember(d => d.imei_no, opt => opt.MapFrom(s => s.imei_no));
            CreateMap<PS_MobileImeiVerificationResponse, MobileImeiVerificationResponse>()
               .ForMember(d => d.imei, opt => opt.MapFrom(s => s.imei))
               .ForMember(d => d.is_valid, opt => opt.MapFrom(s => s.is_valid))
               .ForMember(d => d.brand_name, opt => opt.MapFrom(s => s.brand_name))
               .ForMember(d => d.model_name, opt => opt.MapFrom(s => s.model_name));

            #endregion

            #region MobileOperator
            CreateMap<MobileOperatorRequest, MobileOperatorDto>()
            .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
            .ForMember(d => d.mobile, opt => opt.MapFrom(s => s.mobile));
            CreateMap<PS_MobileOperatorResponse, MobileOperatorResponse>()
               .ForMember(d => d.mobile, opt => opt.MapFrom(s => s.mobile))
               .ForMember(d => d.Operator, opt => opt.MapFrom(s => s.Operator))
               .ForMember(d => d.postpaid, opt => opt.MapFrom(s => s.postpaid));

            #endregion

            #region MobileToUpiVpa
            CreateMap<MobileToUpiVpaRequest, MobileToUpiVpaDto>()
           .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
           .ForMember(d => d.mobile, opt => opt.MapFrom(s => s.mobile));
            CreateMap<PS_MobileToUpiVpaResponse, MobileToUpiVpaResponse>();
            #endregion

            #region NameMatch
            CreateMap<NameMatchRequest, NameMatchDto>()
               .ForMember(d => d.name_1, opt => opt.MapFrom(s => s.name_1))
               .ForMember(d => d.name_2, opt => opt.MapFrom(s => s.name_2));
            CreateMap<PS_NameMatchResponse, NameMatchResponse>();
            #endregion

            #region OCRUpload

            CreateMap<OCRUploadRequest, OCRUploadDto>()
               .ForMember(d => d.type, opt => opt.MapFrom(s => s.type))
               .ForMember(d => d.file, opt => opt.MapFrom(s => s.file))
               .ForMember(d => d.link, opt => opt.MapFrom(s => s.link))
               .ForMember(d => d.back, opt => opt.MapFrom(s => s.back));
            CreateMap<PS_OCRUploadResponse, OCRUploadResponse>();
            #endregion

            #region PanToGst

            CreateMap<PanToGstRequest, PanToGstDto>()
               .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
               .ForMember(d => d.pan_number, opt => opt.MapFrom(s => s.pan_number));
            CreateMap<PS_PanToGstResponse, PanToGstResponse>();
            #endregion

            #region PassportVerify
            CreateMap<PassportVerifyRequest, PassportVerifyDto>()
               .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
               .ForMember(d => d.dob, opt => opt.MapFrom(s => s.dob))
               .ForMember(d => d.id_number, opt => opt.MapFrom(s => s.id_number));
            CreateMap<PS_PassportVerifyResponse, PassportVerifyResponse>();
            #endregion

            #region ProfileGet
            CreateMap<ProfileGetRequest, ProfileGetDto>()
               .ForMember(d => d.client_id, opt => opt.MapFrom(s => s.client_id));
            CreateMap<PS_ProfileGetResponse, ProfileGetResponse>();
            #endregion

            #region RCVerify
            CreateMap<RCVerifyRequest, RCVerifyDto>()
           .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
           .ForMember(d => d.id_number, opt => opt.MapFrom(s => s.id_number));
            CreateMap<PS_RCVerifyResponse, RCVerifyResponse>();
            #endregion

            #region RCAdvanceVerify
            CreateMap<RcAdvanceVerifyRequest, RCAdvanceVerifyDto>()
           .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
           .ForMember(d => d.rc_number, opt => opt.MapFrom(s => s.rc_number));
            CreateMap<PS_RcAdvanceVerifyResponse, RcAdvanceVerifyResponse>();
            #endregion

            #region RCReverse
            CreateMap<RcReverseRequest, RCReverseDto>()
           .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
           .ForMember(d => d.chassis_number, opt => opt.MapFrom(s => s.chassis_number));
            CreateMap<PS_RcReverseResponse, RcReverseResponse>();
            #endregion

            #region ReverseGeoLocation
            CreateMap<ReverseGeoLocationRequest, ReverseGeoLocationDto>()
           .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
           .ForMember(d => d.latitude, opt => opt.MapFrom(s => s.latitude))
           .ForMember(d => d.longitude, opt => opt.MapFrom(s => s.longitude));
            CreateMap<PS_ReverseGeoLocationResponse, ReverseGeoLocationResponse>();
            #endregion

            #region RtoInformation
            CreateMap<RtoInformationRequest, RtoInformationDto>()
           .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
           .ForMember(d => d.rto_code, opt => opt.MapFrom(s => s.rto_code));
            CreateMap<PS_RtoInformationResponse, RtoInformationResponse>();
            #endregion

            #region shopestablishment
            CreateMap<shopestablishmentRequest, shopestablishmentDto>()
           .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
           .ForMember(d => d.state_code, opt => opt.MapFrom(s => s.state_code))
           .ForMember(d => d.shop_number, opt => opt.MapFrom(s => s.shop_number));
            CreateMap<PS_ShopestablishmentResponse, shopestablishmentResponse>();
            #endregion

            #region SingleItrDetails
            CreateMap<SingleItrDetailsRequest, SingleItrDetailsDto>()
            .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
            .ForMember(d => d.client_id, opt => opt.MapFrom(s => s.client_id))
            .ForMember(d => d.itr_id, opt => opt.MapFrom(s => s.itr_id));
            CreateMap<PS_SingleItrDetailsResponse, SingleItrDetailsResponse>();
            #endregion

            #region SingleTwentySixASDetails
            CreateMap<SingleTwentySixASDetailsRequest, SingleTwentySixASDetailsDto>()
           .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
           .ForMember(d => d.client_id, opt => opt.MapFrom(s => s.client_id))
           .ForMember(d => d.tds_id, opt => opt.MapFrom(s => s.tds_id));
            CreateMap<PS_SingleTwentySixASDetailsResponse, SingleTwentySixASDetailsResponse>();
            #endregion

            #region StockPriceVerification
            CreateMap<StockPriceVerificationRequest, StockPriceVerificationDto>()
           .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
           .ForMember(d => d.company, opt => opt.MapFrom(s => s.company));
            CreateMap<PS_StockPriceVerificationResponse, StockPriceVerificationResponse>();
            #endregion

            #region TanLookup
            CreateMap<TanLookupRequest, TanLookupDto>()
           .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
           .ForMember(d => d.company_name, opt => opt.MapFrom(s => s.company_name));
            CreateMap<PS_TanLookupResponse, TanLookupResponse>();
            #endregion

            #region TAN
            CreateMap<TANRequest, TANDto>()
           .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
           .ForMember(d => d.id_number, opt => opt.MapFrom(s => s.id_number));
            CreateMap<PS_TANResponse, TANResponse>();
            #endregion

            #region TANAdvance  
            CreateMap<TANAdvanceRequest, TANAdvanceDto>()
           .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
           .ForMember(d => d.tan_number, opt => opt.MapFrom(s => s.tan_number));
            CreateMap<PS_TANAdvanceResponse, TANAdvanceResponse>();
            #endregion

            #region TelecomValidate
            CreateMap<TelecomValidateRequest, TelecomValidateDto>()
           .ForMember(d => d.id_number, opt => opt.MapFrom(s => s.id_number));
            CreateMap<PS_TelecomValidateResponse, TelecomValidateResponse>();
            #endregion

            #region TelecomDetail
            CreateMap<TelecomDetailRequest, TelecomDetailDto>()
           .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
           .ForMember(d => d.client_id, opt => opt.MapFrom(s => s.client_id))
           .ForMember(d => d.otp, opt => opt.MapFrom(s => s.otp));
            CreateMap<PS_TelecomDetailResponse, TelecomDetailResponse>();
            #endregion

            #region TwentySixASList
            CreateMap<TwentySixASListRequest, TwentySixASListDto>()
           .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
           .ForMember(d => d.client_id, opt => opt.MapFrom(s => s.client_id));
            CreateMap<PS_TwentySixASListResponse, TwentySixASListResponse>();
            #endregion

            #region UPI
            CreateMap<UPIRequest, UPIDto>()
           .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
           .ForMember(d => d.id_number, opt => opt.MapFrom(s => s.id_number));
            CreateMap<PS_UPIResponse, UPIResponse>()
           .ForMember(d => d.upi_id, opt => opt.MapFrom(s => s.upi_id))
           .ForMember(d => d.account_exists, opt => opt.MapFrom(s => s.account_exists))
           .ForMember(d => d.full_name, opt => opt.MapFrom(s => s.full_name))
           .ForMember(d => d.remarks, opt => opt.MapFrom(s => s.remarks))
           .ForMember(d => d.ifsc_details, opt => opt.MapFrom(s => s.ifsc_details));
            #endregion

            #region VoterValidate
            CreateMap<VoterValidateRequest, VoterValidateDto>()
           .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
           .ForMember(d => d.id_number, opt => opt.MapFrom(s => s.id_number));
            CreateMap<PS_VoterValidateResponse, VoterValidateResponse>()
           .ForMember(d => d.input_voter_id, opt => opt.MapFrom(s => s.input_voter_id))
           .ForMember(d => d.epic_no, opt => opt.MapFrom(s => s.epic_no))
           .ForMember(d => d.gender, opt => opt.MapFrom(s => s.gender))
           .ForMember(d => d.state, opt => opt.MapFrom(s => s.state))
           .ForMember(d => d.name, opt => opt.MapFrom(s => s.name))
           .ForMember(d => d.relation_name, opt => opt.MapFrom(s => s.relation_name))
           .ForMember(d => d.relation_type, opt => opt.MapFrom(s => s.relation_type))
           .ForMember(d => d.house_no, opt => opt.MapFrom(s => s.house_no))
           .ForMember(d => d.dob, opt => opt.MapFrom(s => s.dob))
           .ForMember(d => d.age, opt => opt.MapFrom(s => s.age))
           .ForMember(d => d.area, opt => opt.MapFrom(s => s.area))
           .ForMember(d => d.district, opt => opt.MapFrom(s => s.district))
           .ForMember(d => d.additional_check, opt => opt.MapFrom(s => s.additional_check))
           .ForMember(d => d.multiple, opt => opt.MapFrom(s => s.multiple))
           .ForMember(d => d.last_update, opt => opt.MapFrom(s => s.last_update))
           .ForMember(d => d.assembly_constituency, opt => opt.MapFrom(s => s.assembly_constituency))
           .ForMember(d => d.assembly_constituency_number, opt => opt.MapFrom(s => s.assembly_constituency_number))
           .ForMember(d => d.polling_station, opt => opt.MapFrom(s => s.polling_station))
           .ForMember(d => d.part_number, opt => opt.MapFrom(s => s.part_number))
           .ForMember(d => d.part_name, opt => opt.MapFrom(s => s.part_name))
           .ForMember(d => d.slno_inpart, opt => opt.MapFrom(s => s.slno_inpart))
           .ForMember(d => d.ps_lat_long, opt => opt.MapFrom(s => s.ps_lat_long))
           .ForMember(d => d.rln_name_v1, opt => opt.MapFrom(s => s.rln_name_v1))
           .ForMember(d => d.rln_name_v2, opt => opt.MapFrom(s => s.rln_name_v2))
           .ForMember(d => d.rln_name_v3, opt => opt.MapFrom(s => s.rln_name_v3))
           .ForMember(d => d.section_no, opt => opt.MapFrom(s => s.section_no))
           .ForMember(d => d.name_v1, opt => opt.MapFrom(s => s.name_v1))
           .ForMember(d => d.name_v2, opt => opt.MapFrom(s => s.name_v2))
           .ForMember(d => d.name_v3, opt => opt.MapFrom(s => s.name_v3))
           .ForMember(d => d.parliamentary_name, opt => opt.MapFrom(s => s.parliamentary_name))
           .ForMember(d => d.parliamentary_number, opt => opt.MapFrom(s => s.parliamentary_number))
           .ForMember(d => d.st_code, opt => opt.MapFrom(s => s.st_code))
           .ForMember(d => d.parliamentary_constituency, opt => opt.MapFrom(s => s.parliamentary_constituency))
           .ForMember(d => d.id, opt => opt.MapFrom(s => s.id));
            #endregion

            #region AadharOtpsend
            CreateMap<AadhaarSendOtpRequest, AadharSendOtpDto>()
           .ForMember(d => d.id_number, opt => opt.MapFrom(s => s.aadhaar_number));

            CreateMap<PS_AadharSendOtpResponse, AadharSendOtpResponse>()
           .ForMember(d => d.client_id, opt => opt.MapFrom(s => s.client_id))
           .ForMember(d => d.otp_sent, opt => opt.MapFrom(s => s.otp_sent))
           .ForMember(d => d.if_number, opt => opt.MapFrom(s => s.if_number))
           .ForMember(d => d.valid_aadhaar, opt => opt.MapFrom(s => s.valid_aadhaar))
           .ForMember(d => d.status, opt => opt.MapFrom(s => s.status));
            #endregion

            #region AadharOtpVerify
            CreateMap<AadhaarVerifyOtpRequest, AadharVerifyOtpDto>()
          .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
          .ForMember(d => d.client_id, opt => opt.MapFrom(s => s.client_id))
          .ForMember(d => d.otp, opt => opt.MapFrom(s => s.otp));
            CreateMap<PS_AadhaarVerifyOtpResponse, AadhaarVerifyOtpResponse>();
            CreateMap<Address, AadhaarAddress>();
            #endregion

            #region GSTAdvancev2
            CreateMap<GstAdvancev2Request, GstAdvanceV2Dto>()
            .ForMember(d => d.refid, opt => opt.MapFrom(s => s.refid))
            .ForMember(d => d.gst_number, opt => opt.MapFrom(s => s.gst_number));
            CreateMap<PS_GstAdvancev2Response, GstAdvancev2Response>();

            #endregion

        }
    }
}
