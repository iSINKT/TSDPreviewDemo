using System;
// ReSharper disable UnusedMember.Global
namespace TSD.PreviewDemo.Common.Extensions
{
    public static class UriExtensions
    {
        public static bool IsEmpty(this Uri uri)
        {
            uri.ThrowIfNull(nameof(uri));
            return uri.AbsoluteUri == "about:blank";
        }
    }
}