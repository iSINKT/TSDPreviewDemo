using Castle.DynamicProxy;

namespace TSD.PreviewDemo.Common.Interceptors
{
    public class AsyncInterceptorAdapter<TAsyncInterceptor> : AsyncDeterminationInterceptor
        where TAsyncInterceptor : IAsyncInterceptor
    {
        public AsyncInterceptorAdapter(TAsyncInterceptor asyncInterceptor)
            : base(asyncInterceptor)
        { }
    }
}