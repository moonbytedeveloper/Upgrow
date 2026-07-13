namespace VerifyIndia.Application.IServices.Inquiry
{
    public interface IInquiryStatusUpdatable
    {
        Task UpdateInquiryStatusAsync(string uuid, bool isActive, string remark, string actionTakenBy);
    }
}
