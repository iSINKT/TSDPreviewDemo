using System;
using System.Diagnostics;
using TSD.PreviewDemo.Common.Logging;

namespace TSD.PreviewDemo.Common.Utils
{
    public static class ElapsedTimeLoggerLevel
    {
        private static string _level;
        private static readonly object LockObject = new object();
        // ReSharper disable once UseStringInterpolation
        // ReSharper disable InconsistentlySynchronizedField
        public static string Level => string.Format("{0}({1})", _level, _level.Length);

        public static void IncLevel()
        {
            lock (LockObject)
            {
                _level += "*";
            }
        }
        public static void ReduceLevel()
        {
            lock (LockObject)
            {
                if (_level.Length > 0)
                {
                    _level = _level.Substring(0, _level.Length - 1);
                }
            }
        }
    }

    // ReSharper disable UnusedMember.Global
    public class ElapsedTimeLogger : IDisposable
    {
        //private readonly ILogger _logger = DependencyResolver.ResolveLogger();
        private readonly ILogger _logger;
        private readonly string _description;
        private readonly Stopwatch _stopwatch;
         public ElapsedTimeLogger(string description, ILogger logger = null)
        {
            // ReSharper disable once UseNameofExpression
            _logger = logger ?? throw new ArgumentNullException("logger");
            _description = description;
            _stopwatch = Stopwatch.StartNew();
            ElapsedTimeLoggerLevel.IncLevel();
        }

        public void Dispose()
        {
            _logger.Trace("{2} Elapsed time:{0}-{1} ms.", _description, _stopwatch.ElapsedMilliseconds, ElapsedTimeLoggerLevel.Level);
            ElapsedTimeLoggerLevel.ReduceLevel();
            GC.SuppressFinalize(this);
        }
    }
}
