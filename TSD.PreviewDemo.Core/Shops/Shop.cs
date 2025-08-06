using System.Collections.Generic;

namespace TSD.PreviewDemo.Core.Shops
{
    // ReSharper disable UnusedMember.Global
    public class Shop
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public List<ShopLocation> ShopLocations { get; set; }
    }
}
