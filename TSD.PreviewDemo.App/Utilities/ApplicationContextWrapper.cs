using Android.Content;
using Android.OS;
using TSD.PreviewDemo.Common.App;
using TSD.PreviewDemo.Common.Extensions;

namespace TSD.PreviewDemo.App.Utilities
{
    public class ApplicationContextWrapper(Context context) : IApplicationContext
    {
        private readonly Context _context = context.ThrowIfNull(nameof(context));

        public string ExternalFilesDir => _context.GetExternalFilesDir(null)?.AbsolutePath ?? string.Empty;
        public string DownloadsDirectory => _context.GetExternalFilesDir(Environment.DirectoryDownloads)?.AbsolutePath ?? string.Empty;
    }
}