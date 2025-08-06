using System;


namespace TSD.PreviewDemo.Logger
{
    // ReSharper disable UnusedMember.Global
    public class LogFileWrapper : IComparable<LogFileWrapper>
    {
        public DateTime DateTimeWriteFile { get; set; }

        public string LogFileName { get; set; }

        public LogFileWrapper(string logFileName, DateTime dateTimeCreateFile)
        {
            LogFileName = logFileName;
            DateTimeWriteFile = dateTimeCreateFile;
        }

        public int CompareTo(LogFileWrapper obj)
        {
            return obj == null ? 1 : DateTimeWriteFile.CompareTo(obj.DateTimeWriteFile);
        }
    }
}
