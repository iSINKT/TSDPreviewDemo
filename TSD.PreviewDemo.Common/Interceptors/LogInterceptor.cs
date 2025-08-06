using System;
using System.Linq;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using TSD.PreviewDemo.Common.Extensions;
using ILogger = TSD.PreviewDemo.Common.Logging.ILogger;

namespace TSD.PreviewDemo.Common.Interceptors
{
    public class LogInterceptor : AsyncInterceptorBase, IInterceptor
    {
        private readonly ILogger _logger;

        public LogInterceptor(ILogger logger)
        {
            _logger = logger.ThrowIfNull(nameof(logger));
        }

        protected override async Task InterceptAsync(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task> proceed)
        {
            _logger.Trace(
                $"Calling method '{invocation.Method.Name}' with parameters '{string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray())}'... ");
            await proceed(invocation, proceedInfo).ConfigureAwait(false);
        }

        protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
        {
            _logger.Trace(
                $"Calling method '{invocation.Method.Name}' with parameters '{string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray())}'... ");
            var result = await proceed(invocation, proceedInfo).ConfigureAwait(false);
            return result;
        }

        public void Intercept(IInvocation invocation)
        {
            _logger.Trace(
                $"Calling method '{invocation.Method.Name}' with parameters '{string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray())}'... ");
            invocation.Proceed();
        }
    }
}