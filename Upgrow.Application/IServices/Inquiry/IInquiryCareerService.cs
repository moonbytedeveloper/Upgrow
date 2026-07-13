using Upgrow.Application.DTO.Inquiry;

namespace Upgrow.Application.IServices.Inquiry
{
    public interface IInquiryCareerService : IPagedService<InquiryGeneralDto>, IInquiryStatusUpdatable
    {
    }
}
