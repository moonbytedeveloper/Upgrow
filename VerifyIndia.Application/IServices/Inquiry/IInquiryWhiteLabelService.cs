using Upgrow.Application.DTO.Inquiry;

namespace Upgrow.Application.IServices.Inquiry
{
    public interface IInquiryWhiteLabelService : IPagedService<InquiryGeneralDto>, IInquiryStatusUpdatable
    {
    }
}
