using System;
using Autofac.Extras.DynamicProxy;
using TSD.PreviewDemo.Common.Interceptors;

// ReSharper disable UnusedMember.Global
namespace TSD.PreviewDemo.Core.Interfaces
{
    [Intercept(typeof(AsyncInterceptorAdapter<LogInterceptor>))]
    public interface IRaiseError
    {
        event EventHandler<BusinessError> OnError;
    }
}