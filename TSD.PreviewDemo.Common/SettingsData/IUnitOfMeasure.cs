namespace TSD.PreviewDemo.Common.SettingsData
{
    // ReSharper disable UnusedMember.Global
    public interface IUnitOfMeasure
    {
        /// <remarks/>
        int Precision { get; set; }

        /// <remarks/>
        string UnitId { get; set; }

        /// <remarks/>
        string UnitIdExt { get; set; }
    }
}