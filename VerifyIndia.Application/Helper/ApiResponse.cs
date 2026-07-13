using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace VerifyIndia.Application.Helper
{
    public class ApiResponse<T>
    {
        /*public int statuscode { get; set; }*/
        public HttpStatusCode StatusCode { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T Data { get; set; } = CreateDefaultData();

        private static T CreateDefaultData()
        {
            if (typeof(T) == typeof(JsonElement))
            {
                var nullJsonElement = JsonSerializer.SerializeToElement<object?>(null);
                return (T)(object)nullJsonElement;
            }

            return default!;
        }
        public static ApiResponse<T> Ok(T? data = default, string message = "Success", HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new ApiResponse<T>
            {
                StatusCode = statusCode,
                Success = true,
                Message = message,
                Data = data
            };
        }
        public static ApiResponse<T> Fail(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest, T? data = default)
        {
            return new ApiResponse<T>
            {
                StatusCode = statusCode,
                Success = false,
                Message = message,
                Data = data
            };
        }
        public static ApiResponse<T> InternalServerError(string message)
        {
            return new ApiResponse<T>
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Success = false,
                Message = message,
                Data = typeof(T) == typeof(JsonElement)
                    ? (T)(object)JsonSerializer.SerializeToElement<object?>(null)
                    : default!
            };
        }
    }
}