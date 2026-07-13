using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities.Registration;

namespace Upgrow.Domain.IRepositories.Registration
{
    public interface ICustomerConsentDosDontsRepository
    {
        Task<CustomerConsentDosDonts?>
            GetByCustomerUUIDAsync(
                string customerUuid);

        Task AddAsync(
            CustomerConsentDosDonts entity);

        Task UpdateAsync(
            CustomerConsentDosDonts entity);

        Task<bool> HasAcceptedDocumentAsync(
            string customerUuid,
            string documentUuid);
    }
}
