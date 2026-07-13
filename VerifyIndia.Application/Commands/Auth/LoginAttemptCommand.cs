using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.Common;

namespace VerifyIndia.Application.Commands.Auth
{
    public class LoginAttemptCommand : IMasterCommand
    {
        public string? UUID { get; set; }   // required by IMasterCommand

        public string? UserName { get; set; }
        public DateTimeOffset? AttemptTime { get; set; }
        public bool IsSuccess { get; set; }
        public string? FailureReason { get; set; }
        public string? IpAddress { get; set; }
    }
}