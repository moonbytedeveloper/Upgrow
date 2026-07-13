using Upgrow.Application.DTO.Inquiry;

namespace Upgrow.Application.IServices.Inquiry
{
    public interface IInquiryDistributorService : IPagedService<InquiryGeneralDto>, IInquiryStatusUpdatable
    {
    }
}
