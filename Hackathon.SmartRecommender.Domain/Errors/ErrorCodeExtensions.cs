using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Hackathon.SmartRecommender.Domain.Errors
{
    /// <summary>
    /// Error code extensions
    /// </summary>
    public static class ErrorCodeExtensions
    {
        private static readonly ReadOnlyDictionary<EErrorCode, string> ErrorDescriptions;

        static ErrorCodeExtensions()
        {
            var lookup = Enum.GetValues(typeof(EErrorCode))
                .Cast<EErrorCode>()
                .ToDictionary(x => x,
                    x =>
                    {
                        var descriptionAttribute = x.GetType().GetField(x.ToString()).GetCustomAttribute<DescriptionAttribute>();

                        if (descriptionAttribute == null)
                        {
                            throw new InvalidOperationException($"{nameof(EErrorCode)}.{x.ToString()} is missing a {nameof(DescriptionAttribute)}");
                        }

                        return descriptionAttribute.Description;
                    });

            ErrorDescriptions = new ReadOnlyDictionary<EErrorCode, string>(lookup);
        }

        /// <summary>
        /// Get description of an ErrorCode
        /// </summary>
        public static string GetDescription(this EErrorCode error)
        {
            return ErrorDescriptions[error];
        }
    }
}
