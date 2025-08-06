using System.Collections.Generic;
using Newtonsoft.Json;

namespace TSD.PreviewDemo.DataEntities.Barcode
{
    // ReSharper disable UnusedMember.Global
    public class BarcodeGroup 
    { 
        [JsonProperty("ID")]
        public string Id { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public List<BarcodeGroupRules> BarcodeRules { get; set; }
    }
}
