using System.Collections.Generic;

namespace TSD.PreviewDemo.Core.StoreAcceptance
{
    // ReSharper disable UnusedMember.Global
    public class JobList : Entity
    {
        public string CargoBarcode { get; set; }
        public List<Job> Jobs { get; set; } = new List<Job>();
        public List<ColorData> Colors { get; set; } = new List<ColorData>();
        public string OrderNumber { get; set; }
        public string TopJobsForSelection { get; set; }
    }

    public class ColorData
    {
        public int FontColor { get; set; }
        public int ObjectColor { get; set; }
        public int ObjectId { get; set; }
    }

    public class Job
    {
        public string AlcoType { get; set; }
        public string AllowPick { get; set; }
        public string GateNum { get; set; }
        public bool Indicator { get; set; }
        public string InvoiceNum { get; set; }
        public string JobId { get; set; }
        public string JobName { get; set; }
        public string JobNum { get; set; }
        public string OrdinalNum { get; set; }
    }
}
