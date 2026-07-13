using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.Common.Results
{
    public sealed class Result<T> : Result
    {
        private Result(
            bool isSuccess,
            T? data,
            ErrorType errorType,
            string message,
            IReadOnlyCollection<Error>? errors)
            : base(
                isSuccess,
                errorType,
                message,
                errors)
        {
            Data = data;
        }

        public T? Data { get; }

        public static Result<T> Success(
            T data,
            string message = "Success")
            => new(
                true,
                data,
                ErrorType.None,
                message,
                null);

        public static Result<T> Failure(
            ErrorType errorType,
            string message,
            IReadOnlyCollection<Error>? errors = null)
            => new(
                false,
                default,
                errorType,
                message,
                errors);

        public static Result<T> Failure(
            ErrorType errorType,
            string message,
            T data,
            IReadOnlyCollection<Error>? errors = null)
            => new(
                false,
                data,
                errorType,
                message,
                errors);
    }
}
