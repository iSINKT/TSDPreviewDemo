using Newtonsoft.Json;

namespace TSD.PreviewDemo.DataEntities.Barcode
{
    // ReSharper disable UnusedMember.Global
    public class BarcodeGroupRules 
    {
        public string BarcodeStandard { get; set; }
        public string Value { get; set; }
        [JsonProperty("ID")]
        public string Id { get; set; }
    }
}
