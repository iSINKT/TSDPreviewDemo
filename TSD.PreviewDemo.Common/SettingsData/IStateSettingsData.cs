namespace TSD.PreviewDemo.Common.SettingsData
{
    // ReSharper disable UnusedMember.Global
    public interface IStateSettingsData
    {
        string EntryPoint { get; set; }

        /// <remarks/>
        int KeepAliveInterval { get; set; }

        /// <remarks/>
        int LocalDateTimeUpdateInterval { get; set; }

        /// <remarks/>
        int LocalDateTimeValidDeviation { get; set; }

        /// <remarks/>
        string LogIsEnabled { get; set; }

        /// <remarks/>
        string Manifest { get; set; }

        int ServiceRequestTimeout { get; set; }

        /// <remarks/>
        string ThresholdOfLog { get; set; }
        IStateContextVariable[] ContextVariablesSettings { get; set; }

        IUnitOfMeasure[] UnitOfMeasureSettings { get; set; }
        IDataInfoRow[] AboutApp { get; set; }
    }
}
