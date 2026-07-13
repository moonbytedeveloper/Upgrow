using System.ComponentModel.DataAnnotations;

namespace VerifyIndia.Application.Common
{
    public static class ValidationError
    {
        public static ValidationResult For(string memberName, string message)
            => new(message, new[] { memberName });

        public static ValidationResult For(string[] memberNames, string message)
            => new(message, memberNames);
    }
}