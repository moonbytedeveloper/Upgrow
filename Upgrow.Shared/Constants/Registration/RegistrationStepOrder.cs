using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticateIndia.Shared.Constants.Registration
{
    public static class RegistrationStepOrder
    {
        public static readonly List<string> Steps =
        [
            RegistrationSteps.SELECT_ACCOUNT_TYPE,
            RegistrationSteps.VERIFY_AADHAAR,
            RegistrationSteps.BASIC_INFO,
            RegistrationSteps.VIDEO_KYC,
            RegistrationSteps.TERMS,
            RegistrationSteps.DOS_DONTS,
            RegistrationSteps.DASHBOARD
        ]; 

        public static int GetIndex(
            string step)
        {
            return Steps.IndexOf(step);
        }
    }
}
