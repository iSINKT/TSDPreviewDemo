using System.Collections.Generic;

namespace TSD.PreviewDemo.Core.BarCodes
{
    // ReSharper disable UnusedMember.Global
    public class Group 
    { 
        public string Id { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public List<Rule> BarcodeRules { get; set; }
    }
}
