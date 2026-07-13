using Upgrow.Application.DTO.Inquiry;

namespace Upgrow.Application.IServices.Inquiry
{
    public interface IInquiryGeneralService : IPagedService<InquiryGeneralDto>, IInquiryStatusUpdatable
    {
    }
}
