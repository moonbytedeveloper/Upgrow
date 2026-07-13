namespace VerifyIndia.Domain.Entities
{
    public class CustomerConsent
    {
        public decimal Id { get; set; }
        public string UUID { get; set; } = string.Empty;
        public string CustomerUUID { get; set; } = string.Empty;
        public string? PolicyUUID { get; set; }
        public string? PolicyContentHash { get; set; }        
        public string? PolicyVersion { get; set; }
        public bool ConsentGiven { get; set; }
        public DateTimeOffset? SignedAt { get; set; }
        public string? IpAddress { get; set; }
        public string? Platform { get; set; }
        public string? SignId { get; set; }
        public string? Signature { get; set; }
        public string? LatLong { get; set; }
    }
}
