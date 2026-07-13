using Upgrow.Application.Commands.Master;
using Upgrow.Application.IServices.Master;

namespace Upgrow.Application.Commands.Auth
{
    public class WL_LoginLogoutCommand : IMasterCommand
    {
        public string? UUID { get; set; }
        public bool IsActive { get; set; }
    }
}