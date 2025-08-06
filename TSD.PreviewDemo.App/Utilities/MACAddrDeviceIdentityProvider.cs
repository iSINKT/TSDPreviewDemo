using System;
using Java.Net;
using Java.Util;
using TSD.PreviewDemo.Common.Platform;

namespace TSD.PreviewDemo.App.Utilities
{
    // ReSharper disable once UnusedMember.Global
    public class MacAddressDeviceIdentityProvider : IDeviceIdentityProvider
    {
        public string DeviceId { get; } = GetMacAddress();

        private static string GetMacAddress()
        {
            var mac = "02:00:00:00:00";
            if (NetworkInterface.NetworkInterfaces == null) return mac;

            var all = Collections.List(NetworkInterface.NetworkInterfaces);
            foreach (var element in all)
            {
                var nif = (NetworkInterface)element;
                if (string.IsNullOrWhiteSpace(nif.Name!) || nif.Name.Equals("wlan0")) continue;
                var bytes = nif.GetHardwareAddress();
                if (bytes != null)
                    mac = BitConverter.ToString(bytes);
            }
            return mac;
        }
    }
}
