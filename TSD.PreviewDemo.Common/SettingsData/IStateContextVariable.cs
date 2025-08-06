// ReSharper disable UnusedMember.Global
namespace TSD.PreviewDemo.Common.SettingsData
{
    public interface IStateContextVariable
    {
        string Name { get; set; }
        string Type { get; set; }
        string Value { get; set; }
    }
}
