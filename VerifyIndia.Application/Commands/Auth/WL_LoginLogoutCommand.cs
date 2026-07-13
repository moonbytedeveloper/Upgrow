using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.IServices.Master;

namespace VerifyIndia.Application.Commands.Auth
{
    public class WL_LoginLogoutCommand : IMasterCommand
    {
        public string? UUID { get; set; }
        public bool IsActive { get; set; }
    }
}