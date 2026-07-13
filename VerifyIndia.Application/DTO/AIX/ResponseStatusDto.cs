namespace VerifyIndia.Application.DTO.AIX
{
    public class ResponseStatusDto
    {
        public string? UUID { get; set; }
        public string? Title { get; set; }
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsActive { get; set; }
    }
}