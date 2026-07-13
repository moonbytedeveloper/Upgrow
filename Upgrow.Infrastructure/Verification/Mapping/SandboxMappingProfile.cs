using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.DTO.Verification.Common.Request;
using Upgrow.Application.DTO.Verification.Common.Response;
using Upgrow.Application.DTO.Verification.Sandbox;
using Upgrow.Application.DTO.Verification.Sandbox.Response;

namespace Upgrow.Infrastructure.Verification.Mapping
{
    public class SandboxMappingProfile : Profile
    {
        public SandboxMappingProfile()
        {
            // PAN: Internal → Sandbox
            CreateMap<PanVerifyRequest, SB_PanVerifyDto>()
                .ForMember(d => d.pan, opt => opt.MapFrom(s => s.pan))
                .ForMember(d => d.name_as_per_pan, opt => opt.MapFrom(s => s.name_as_per_pan))
                .ForMember(d => d.date_of_birth, opt => opt.MapFrom(s => s.date_of_birth))
                .ForMember(d => d.consent, opt => opt.MapFrom(s => s.consent))
                .ForMember(d => d.reason, opt => opt.MapFrom(s => s.reason));

            // PAN: Sprint → Internal
            /*CreateMap<SprintPanResponse, PanVerifyResponse>()
                .ForMember(d => d.PanNumber, opt => opt.MapFrom(s => s.PanNumber))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.FullName));*/

            #region IFSC
            // IFSC: Request
            CreateMap<IfscVerifyRequest, SB_IfscVerifyDto>()
                .ForMember(d => d.ifsc, opt => opt.MapFrom(s => s.ifsc));
            // IFSC: Response
            CreateMap<IfscVerifyResponse, SB_IfscVerifyResponse>();
            #endregion

            #region PAN-Aadhaar Link
            // IFSC: Request
            CreateMap<PanAadharLinkRequest, SB_PanAadharLinkDto>()
                .ForMember(d => d.pan, opt => opt.MapFrom(s => s.pan))
                .ForMember(d => d.aadhaar_number, opt => opt.MapFrom(s => s.aadhaar_number))
                .ForMember(d => d.consent, opt => opt.MapFrom(s => s.consent))
                .ForMember(d => d.reason, opt => opt.MapFrom(s => s.reason));
            // IFSC: Response
            CreateMap<PanAadharLinkResponse, SB_PanAadharLinkResponse>();
            #endregion

            #region Search GST
            // Search GST: Request
            CreateMap<SearchGstRequest, SB_SearchGstDto>()
                .ForMember(d => d.pan, opt => opt.MapFrom(s => s.pan));
            // Search GST: Response
            CreateMap<SearchGstResponse, SB_SearchGstResponse>();
            #endregion

            #region Penny Drop
            // Penny Drop: Request
            CreateMap<PennyDropVerifyRequest, SB_PennyDropVerifyDto>()
                .ForMember(d => d.ifsc, opt => opt.MapFrom(s => s.ifsc))
                .ForMember(d => d.account_number, opt => opt.MapFrom(s => s.account_number));
            // Penny Drop: Response
            CreateMap<PennyDropVerifyResponse, SB_PennyDropVerifyResponse>();
            #endregion

            #region PennyLess
            // PennyLess : Request
            CreateMap<PennyLessVerifyRequest, SB_PennyLessVerifyDto>()
                .ForMember(d => d.ifsc, opt => opt.MapFrom(s => s.ifsc_code))
                .ForMember(d => d.account_number, opt => opt.MapFrom(s => s.account_number));
            // PennyLess : Response
            CreateMap<PennyLessVerifyResponse, SB_PennyLessVerifyResponse>();
            #endregion

            #region AadhaarSendOtp
            // Aadhar Send OTP: Request
            CreateMap<AadhaarSendOtpRequest, SB_AadhaarSendOtpDto>()
           .ForMember(d => d.aadhaar_number, opt => opt.MapFrom(s => s.aadhaar_number))
           .ForMember(d => d.reason, opt => opt.MapFrom(s => s.reason))
           .ForMember(d => d.consent, opt => opt.MapFrom(s => s.consent));
            // Aadhar Send OTP: Response
            CreateMap<SB_AadharSendOtpResponse, AadharSendOtpResponse>();
            #endregion

            #region AadhaarVerifyOtp
            CreateMap<AadhaarVerifyOtpRequest, SB_AadhaarVerifyOtpDto>()
          .ForMember(d => d.reference_id, opt => opt.MapFrom(s => s.refid))
          .ForMember(d => d.otp, opt => opt.MapFrom(s => s.otp));
            // Aadhar Send OTP: Response
            CreateMap<SB_AadharSendOtpResponse, AadharSendOtpResponse>();
            #endregion

        }
    }
}
