using System;
using Serilog;

namespace TwitchChatDownloader.Logging
{
    public class Log<T> : ILog<T>
    {
        private readonly ILogger _logger;
        private readonly string _type;
        public Log(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _type = typeof(T).Name;
        }

        public void Info(string info) =>
            _logger.Information(AppendType(info));

        public void Warn(string warn) =>
            _logger.Warning(AppendType(warn));

        public void Error(string error) =>
            _logger.Error(AppendType(error));

        private string AppendType(string message) =>
            $"Type: {_type} {message}";
    }
}
