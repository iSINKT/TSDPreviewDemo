using Android.Content;
using Android.Provider;
using TSD.PreviewDemo.Common.Platform;

namespace TSD.PreviewDemo.App.Utilities
{
    public class AndroidDeviceIdentityProvider(Context context) : IDeviceIdentityProvider
    {
        public string DeviceId { get; } = Settings.Secure.GetString(context.ContentResolver, Settings.Secure.AndroidId);
    }
}
