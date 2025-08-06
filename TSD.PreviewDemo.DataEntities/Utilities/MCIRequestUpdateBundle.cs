using Newtonsoft.Json;
using System;
// ReSharper disable InconsistentNaming

namespace TSD.PreviewDemo.DataEntities.Utilities
{
    public class MCIRequestUpdateBundle
    {
        [JsonProperty("bundleId")]
        public Guid SoftwareBundleId { get; set; }

        [JsonProperty("bundleVersion")]
        public string SoftwareBundleVersion { get; set; }

        [JsonProperty("bundleInnerVersion")]
        public string SoftwareBundleInnerVersion { get; set; }

        [JsonProperty("bundleName")]
        public string SoftwareBundleName { get; set; }
    }
}
