using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.Common;

namespace VerifyIndia.Application.Commands.Auth
{
    public class LoginLogoutCommand : IMasterCommand
    {
        public string? UUID { get; set; }   // required by IMasterCommand

        public string? UserUUID { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public string? Activity { get; set; }
        public string? IpAddress { get; set; }
    }
}