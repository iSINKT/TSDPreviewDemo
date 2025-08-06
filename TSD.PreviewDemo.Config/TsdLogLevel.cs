using System;
// ReSharper disable UnusedMember.Global

namespace TSD.PreviewDemo.Config
{
    public class LogLevel
    {   
        /// <summary>
        /// Trace log level.
        /// </summary>
        public static readonly LogLevel Trace = new LogLevel("Trace", 0);

        /// <summary>
        /// Debug log level.
        /// </summary>
        public static readonly LogLevel Debug = new LogLevel("Debug", 1);

        /// <summary>
        /// Info log level.
        /// </summary>
        public static readonly LogLevel Info = new LogLevel("Info", 2);

        /// <summary>
        /// Warn log level.
        /// </summary>
        public static readonly LogLevel Warn = new LogLevel("Warn", 3);

        /// <summary>
        /// Error log level.
        /// </summary>
        public static readonly LogLevel Error = new LogLevel("Error", 4);

        /// <summary>
        /// Fatal log level.
        /// </summary>
        public static readonly LogLevel Fatal = new LogLevel("Fatal", 5);

        /// <summary>
        /// Off log level.
        /// </summary>
        public static readonly LogLevel Off = new LogLevel("Off", 6);

        // to be changed into public in the future.
        private LogLevel(string name, int ordinal)
        {
            Name = name;
            Ordinal = ordinal;
        }

        /// <summary>
        /// Gets the name of the log level.
        /// </summary>
        public string Name { get; }

        internal static LogLevel MaxLevel => Fatal;

        internal static LogLevel MinLevel => Trace;

        /// <summary>
        /// Gets the ordinal of the log level.
        /// </summary>
        internal int Ordinal { get; }

        //public enum LogLevel
        //{
        //    Info,
        //    Trace, 
        //    Warning,
        //    Error,
        //    Fatal
        //}
        public static LogLevel FromString(string levelName)
        {
            if (levelName == null)
            {
                throw new ArgumentNullException(nameof(levelName));
            }

            if (levelName.Equals("Trace", StringComparison.OrdinalIgnoreCase))
            {
                return Trace;
            }

            if (levelName.Equals("Debug", StringComparison.OrdinalIgnoreCase))
            {
                return Debug;
            }

            if (levelName.Equals("Info", StringComparison.OrdinalIgnoreCase))
            {
                return Info;
            }

            if (levelName.Equals("Warn", StringComparison.OrdinalIgnoreCase))
            {
                return Warn;
            }

            if (levelName.Equals("Error", StringComparison.OrdinalIgnoreCase))
            {
                return Error;
            }

            if (levelName.Equals("Fatal", StringComparison.OrdinalIgnoreCase))
            {
                return Fatal;
            }

            if (levelName.Equals("Off", StringComparison.OrdinalIgnoreCase))
            {
                return Off;
            }

            throw new ArgumentException("Unknown log level: " + levelName);
        }
    }
}
