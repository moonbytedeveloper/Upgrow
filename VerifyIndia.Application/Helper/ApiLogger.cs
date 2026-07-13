using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace VerifyIndia.Application.Helper
{
    public class ApiLogger
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiLogger(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task LogApiExecutionAsync(
            object requestData,
            object response,            
            string apiEndpoint,
            int statusCode,
            string httpMethod = "POST")
        {
            try
            {
                var systemIp = _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "Unknown";
                var now = DateTime.UtcNow;

                var apiLog = new ApiLogs
                {
                    ExecutionTime = now,
                    RequestData = JsonSerializer.Serialize(requestData),
                    Response = JsonSerializer.Serialize(response),                   
                    ApiEndpoint = apiEndpoint,
                    HttpMethod = httpMethod,
                    StatusCode = statusCode,
                    CreatedAt = now
                };

                _dbContext.ApiLogs.Add(apiLog);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error logging API execution: {ex.Message}");
            }
        }
    }
}