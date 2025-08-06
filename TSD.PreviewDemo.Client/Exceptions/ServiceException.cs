using System;
using TSD.PreviewDemo.DataLayer.Services;
// ReSharper disable UnusedMember.Global

namespace TSD.PreviewDemo.Client.Exceptions
{
    public class ServiceException(
        Uri uri,
        ServiceType serviceType,
        string message,
        string exceptionCode,
        Exception innerException)
        : Exception(message, innerException)
    {
        public Uri Uri { get; } = uri;
        public  ServiceType ServiceType { get;} = serviceType;
        public string ExceptionCode { get; } = $"SERVICE_ERROR_{exceptionCode}";
    }
}