using VerifyIndia.Application.DTO.Inquiry;

namespace VerifyIndia.Application.IServices.Inquiry
{
    public interface IInquiryGeneralService : IPagedService<InquiryGeneralDto>, IInquiryStatusUpdatable
    {
    }
}
