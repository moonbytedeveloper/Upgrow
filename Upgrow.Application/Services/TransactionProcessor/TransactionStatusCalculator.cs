using Upgrow.Shared.Constants.TransactionDocument;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Interfaces.TransactionProcessor;
using Upgrow.Domain.Entities;

namespace Upgrow.Application.Services.TransactionProcessor
{
    public class TransactionStatusCalculator
        : ITransactionStatusCalculator
    {
        public void Recalculate(
            Transaction transaction,
            IEnumerable<TransactionDetail> details)
        {
            var detailList = details.ToList();

            if (detailList.All(x =>
                    x.ProcessingStatus ==
                    TransactionDetailProcessingStatusConstants.Completed))
            {
                transaction.ProcessingStatus =
                    TransactionProcessingStatusConstants.Completed;
                return;
            }

            if (detailList.Any(x =>
                    x.ProcessingStatus ==
                    TransactionDetailProcessingStatusConstants.WaitingForCustomerInput))
            {
                transaction.ProcessingStatus =
                    TransactionProcessingStatusConstants.PendingUserAction;
                return;
            }

            if (detailList.All(x =>
                    x.ProcessingStatus ==
                    TransactionDetailProcessingStatusConstants.Failed))
            {
                transaction.ProcessingStatus =
                    TransactionProcessingStatusConstants.Failed;
                return;
            }

            if (detailList.Any(x =>
                    x.ProcessingStatus ==
                    TransactionDetailProcessingStatusConstants.Completed)
                &&
                detailList.Any(x =>
                    x.ProcessingStatus ==
                    TransactionDetailProcessingStatusConstants.Failed))
            {
                transaction.ProcessingStatus =
                    TransactionProcessingStatusConstants.PartiallyCompleted;
                return;
            }

            if (detailList.Any(x =>
                    x.ProcessingStatus ==
                    TransactionDetailProcessingStatusConstants.Pending))
            {
                transaction.ProcessingStatus =
                    TransactionProcessingStatusConstants.Processing;
                return;
            }

            transaction.ProcessingStatus =
                TransactionProcessingStatusConstants.PartiallyCompleted;
        }
    }
}
