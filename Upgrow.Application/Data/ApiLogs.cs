namespace Upgrow
{
    public class ApiLogs
    {
        public int Id { get; set; }
        public DateTime ExecutionTime { get; set; }
        public string RequestData { get; set; }
        public string Response { get; set; }        
        public string ApiEndpoint { get; set; }
        public string HttpMethod { get; set; }
        public int? StatusCode { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
