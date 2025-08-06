using System;

namespace TSD.PreviewDemo.Core.Utility
{
    // ReSharper disable UnusedMember.Global
    public enum BundleState
    {
        UpdateNotRequired,
        UpdateRequired,
        UpdateFromDiskRequired
    }

    public class UpdateBundle
    {
        public Guid SoftwareBundleId { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string InnerVersion { get; set; }
        public string Path { get; set; }
        public string Hash { get; set; }
        public string Command { get; set; }
        public string LocalBundleFilename => $"{Name}_{Version}.apk";
        public BundleState UpdateState { get; set; }
    }
}
