using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.Verification.Interfaces
{
    public interface IVerificationStepResolver
    {
        string GetPreviousVerificationCode(
            string currentVerificationCode);
    }
}
