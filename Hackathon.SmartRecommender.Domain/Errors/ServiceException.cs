using System;
using System.Collections.Generic;
using System.Text;

namespace Hackathon.SmartRecommender.Domain.Errors
{
    /// <summary>
    /// A service exception
    /// </summary>
    public class ServiceException : Exception
    {
        /// <summary>
        /// A canonical error code that represents the business error.
        /// </summary>
        public EErrorCode ErrorCode { get; }

        /// <summary>
        /// Detail
        /// </summary>
        public string Detail { get; }

        /// <summary>
        /// Creates service exception
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="detail"></param>
        public ServiceException(EErrorCode errorCode, string detail = null) : this(errorCode, null, detail)
        {
        }

        /// <summary>
        /// Creates service exception
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="innerException"></param>
        /// <param name="detail"></param>
        public ServiceException(EErrorCode errorCode, Exception innerException, string detail = null) : base(
            GetMessage(errorCode, innerException, detail), innerException)
        {
            ErrorCode = errorCode;
            Detail = detail;
        }

        private static string GetMessage(EErrorCode errorCode, Exception innerException, string detail)
        {
            var message = $"Code: {errorCode}";

            if (innerException != null)
            {
                var innerExceptionMessage = string.Empty;
                if (!string.IsNullOrEmpty(innerException.Message))
                {
                    innerExceptionMessage = $" - {innerException.Message}";
                }

                message += $"\nInner Exception: {innerException.GetType()}{innerExceptionMessage}";
            }

            if (detail != null)
            {
                message += $"\nDetail: {detail}";
            }

            return message;
        }
    }
}
