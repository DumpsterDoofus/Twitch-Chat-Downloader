using System;
using System.IO;
using System.Threading.Tasks;
using Serilog;
using TwitchChatDownloader.Interfaces;
using TwitchChatDownloader.Models;

namespace TwitchChatDownloader.Implementations
{
    internal class LoggingSrtFileWriter : ISrtFileWriter
    {
        private readonly ISrtFileWriter _srtFileWriter;
        private readonly ILogger _logger;

        public LoggingSrtFileWriter(ISrtFileWriter srtFileWriter, ILogger logger)
        {
            _srtFileWriter = srtFileWriter ?? throw new ArgumentNullException(nameof(srtFileWriter));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<FileInfo> Write(InternalVideo internalVideo, string content)
        {
            var fileInfo = await _srtFileWriter.Write(internalVideo, content).ConfigureAwait(false);
            _logger.Information($"Wrote SRT file to {fileInfo.FullName}.");
            return fileInfo;
        }
    }
}
