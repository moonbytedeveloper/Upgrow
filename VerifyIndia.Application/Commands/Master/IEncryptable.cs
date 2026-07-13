using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.Commands.Master
{
    public interface IEncryptable
    {
        string SensitiveValue { get; set; }
    }
}
