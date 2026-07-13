using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VerifyIndia.Application.Helper;
using VerifyIndia.Application.IServices.Mapper;

namespace VerifyIndia.Application.Services.Mapper
{
    public sealed class SprintMapper : IProviderMapper
    {
        private static readonly JsonElement NullElement =
            JsonDocument.Parse("null").RootElement.Clone();

        public ApiResponse<JsonElement> Map(JsonElement root)
        {
            int statusCode = 500;
            bool success = false;
            string? message = "Error";

            if (root.TryGetProperty("statuscode", out var sc) && sc.ValueKind == JsonValueKind.Number)
                statusCode = sc.GetInt32();

            if (root.TryGetProperty("status", out var st) &&
                (st.ValueKind == JsonValueKind.True || st.ValueKind == JsonValueKind.False))
                success = st.GetBoolean();

            if (root.TryGetProperty("message", out var msg) && msg.ValueKind == JsonValueKind.String)
                message = msg.GetString();

            JsonElement data;

            if (root.TryGetProperty("data", out var d))
            {
                data = d.Clone();
            }
            else
            {
                data = NullElement;
            }

            return new ApiResponse<JsonElement>
            {
                StatusCode = (HttpStatusCode)statusCode,
                Success = success,
                Message = message,
                Data = data
            };
        }
    }
}
