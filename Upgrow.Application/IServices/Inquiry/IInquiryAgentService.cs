using Upgrow.Application.DTO.Inquiry;

namespace Upgrow.Application.IServices.Inquiry
{
    public interface IInquiryAgentService : IPagedService<InquiryGeneralDto>, IInquiryStatusUpdatable
    {
        Task ConvertToAgentAsync(string uuid, string userId);
    }
}
