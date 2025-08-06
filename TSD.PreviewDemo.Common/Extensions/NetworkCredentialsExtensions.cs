using System.Net;

namespace TSD.PreviewDemo.Common.Extensions
{
    // ReSharper disable UnusedMember.Global
    public static class NetworkCredentialsExtensions
    {
        public static bool IsEmpty(this NetworkCredential credential)
        {
            credential.ThrowIfNull(nameof(credential));
            return string.IsNullOrEmpty(credential.Domain)
                   && string.IsNullOrEmpty(credential.UserName)
                   && string.IsNullOrEmpty(credential.Password);
        }
    }
}