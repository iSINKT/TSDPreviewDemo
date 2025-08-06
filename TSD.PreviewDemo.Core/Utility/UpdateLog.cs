using System;

namespace TSD.PreviewDemo.Core.Utility
{
    // ReSharper disable UnusedMember.Global
    public class UpdateLog
    {
        public string DeviceId { get; set; }
        public Guid BundleId { get; set; }
        public string BundleVersion { get; set; }
        public int SdkVersion { get; set; }
        public string Command { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
