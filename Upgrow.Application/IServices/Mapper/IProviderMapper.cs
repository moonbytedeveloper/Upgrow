using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Upgrow.Application.Helper;

namespace Upgrow.Application.IServices.Mapper
{
    public interface IProviderMapper
    {
        ApiResponse<JsonElement> Map(JsonElement root);
    }
}
