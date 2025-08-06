using System;
using TSD.PreviewDemo.DataLayer.Services;

namespace TSD.PreviewDemo.DataLayer.Interfaces.Services
{
    // ReSharper disable UnusedMember.Global
    public interface IServiceRecord
    {
        bool IsActive { get; set; }
        Uri Url { get; set; }
        ServiceType SrvType { get; set; }
        string SrvName { get; set; }
    }
}