using VerifyIndia.Application.DTO.Inquiry;

namespace VerifyIndia.Application.IServices.Inquiry
{
    public interface IInquiryAgentService : IPagedService<InquiryGeneralDto>, IInquiryStatusUpdatable
    {
        Task ConvertToAgentAsync(string uuid, string userId);
    }
}
