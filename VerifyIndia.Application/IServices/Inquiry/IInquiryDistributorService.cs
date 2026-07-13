using VerifyIndia.Application.DTO.Inquiry;

namespace VerifyIndia.Application.IServices.Inquiry
{
    public interface IInquiryDistributorService : IPagedService<InquiryGeneralDto>, IInquiryStatusUpdatable
    {
    }
}
