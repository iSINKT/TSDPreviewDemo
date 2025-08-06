using System;

namespace TSD.PreviewDemo.Common.Extensions
{
    public static class NullValidationExtensions
    {
        // ReSharper disable once UnusedMember.Global
        public static string ThrowIfNullOrEmpty(this string strToValidate, string paramName)
        {
            if (string.IsNullOrWhiteSpace(strToValidate))
                throw new ArgumentNullException(paramName);
            return strToValidate;
        }

        public static T ThrowIfNull<T>(this T val, string paramName)
        {
                if (val == null)
                    throw new ArgumentNullException(paramName);
                return val;
        }
    }
}
