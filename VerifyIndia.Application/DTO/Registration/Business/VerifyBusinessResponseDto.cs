using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Registration.Business
{
    public sealed class VerifyBusinessResponseDto
    {
        public RegistrationStateDto RegistrationState
        {
            get;
            set;
        } = new();

        public string? BusinessName
        {
            get;
            set;
        }

        public List<BusinessDirectorDto> Directors
        {
            get;
            set;
        } = [];
    }

    public sealed class BusinessDirectorDto
    {
        public string UUID
        {
            get;
            set;
        } = string.Empty;

        public string Name
        {
            get;
            set;
        } = string.Empty;
    }
}
