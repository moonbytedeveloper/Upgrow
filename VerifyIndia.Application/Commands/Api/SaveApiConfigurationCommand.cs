using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;

namespace VerifyIndia.Application.Commands.Api
{
    public class SaveApiConfigurationCommand
    {
        public MasterApiCommand Api { get; set; } = new();

        public ApiEndpointCommand ApiEndpoint { get; set; } = new();

        public ApiEndpointCommand SecondApiEndpoint { get; set; } = new();

        public string? SelectedComponentUUID { get; set; }

        public string? SelectedSecondComponentUUID { get; set; }

        public List<ProviderConfigurationCommand>
            ProviderMappings
        { get; set; } = new();
    }

    public class ProviderConfigurationCommand
    {
        public string? ProviderUUID { get; set; }

        public string? ProviderName { get; set; }

        public bool SupportsApi { get; set; }

        public bool IsActive { get; set; }

        public decimal? Priority { get; set; }
    }
}
