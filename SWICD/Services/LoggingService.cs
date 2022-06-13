using Microsoft.Extensions.Logging;
using SWICD.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWICD.Services
{
    internal class LoggingService
    {
        public static LoggingService Instance { get; private set; } = new LoggingService();
        private static List<LogEntryModel> _logEntries = new List<LogEntryModel>();
        private string file = "driver_log.log";

        public LoggingService()
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            folder = Path.Combine(folder, "SWICD");
            
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            file = Path.Combine(folder, file);
        }

        public event EventHandler<LogEntryModel> OnNewLogEntry;

        public void Log(LogLevel level, string message)
        {
            var entry = new LogEntryModel()
            {
                LogLevel = level,
                Message = message,
                Time = DateTime.Now,
            };
            _logEntries.Add(entry);
            _ = Task.Run(() => OnNewLogEntry?.Invoke(this, entry));
            File.AppendAllText(file, $"[{entry.Time}][{level}]: {message}\r\n"); 
        }

        public string GetLogString()
        {
            return File.ReadAllText(file);
        }

        public static void LogInformation(string message) => Instance.Log(LogLevel.Information, message);
        public static void LogWarning(string message) => Instance.Log(LogLevel.Warning, message);
        public static void LogError(string message) => Instance.Log(LogLevel.Error, message);
        public static void LogDebug(string message) => Instance.Log(LogLevel.Debug, message);
        public static void LogCritical(string message) => Instance.Log(LogLevel.Critical, message);
    }
}
