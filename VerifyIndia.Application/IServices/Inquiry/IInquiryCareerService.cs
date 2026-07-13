using VerifyIndia.Application.DTO.Inquiry;

namespace VerifyIndia.Application.IServices.Inquiry
{
    public interface IInquiryCareerService : IPagedService<InquiryGeneralDto>, IInquiryStatusUpdatable
    {
    }
}
