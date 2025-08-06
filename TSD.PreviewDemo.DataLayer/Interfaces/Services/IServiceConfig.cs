using System;

namespace TSD.PreviewDemo.DataLayer.Interfaces.Services
{
    // ReSharper disable UnusedMember.Global
    public interface IServiceConfig
    {
        string Name { get; }
        Uri Url { get; }
        bool IsActive { get; set; }
    }
}
