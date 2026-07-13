using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Upgrow.Application.Verification.Interfaces
{
    public interface IVerificationRequestFactory
    {
        object? CreateRequest(
            string verificationCode,
            string? requestJson);

        object? CreateRequest(
            string verificationCode,
            JsonElement requestJson);
    }
}
