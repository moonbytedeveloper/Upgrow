using VerifyIndia.Application.DTO.Inquiry;

namespace VerifyIndia.Application.IServices.Inquiry
{
    public interface IInquiryWhiteLabelService : IPagedService<InquiryGeneralDto>, IInquiryStatusUpdatable
    {
    }
}
