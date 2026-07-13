using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.Common.Results
{
    public class Result
    {
        protected Result(
            bool isSuccess,
            ErrorType errorType,
            string message,
            IReadOnlyCollection<Error>? errors = null)
        {
            IsSuccess = isSuccess;
            ErrorType = errorType;
            Message = message;
            Errors = errors;
        }

        public bool IsSuccess { get; }

        public ErrorType ErrorType { get; }

        public string Message { get; }

        public IReadOnlyCollection<Error>? Errors { get; }

        public static Result Success(
            string message = "Success")
            => new(
                true,
                ErrorType.None,
                message);

        public static Result Failure(
            ErrorType errorType,
            string message,
            IReadOnlyCollection<Error>? errors = null)
            => new(
                false,
                errorType,
                message,
                errors);
    }
}
