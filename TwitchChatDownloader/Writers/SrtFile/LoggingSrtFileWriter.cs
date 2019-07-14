using System;
using System.IO;
using System.Threading.Tasks;
using TwitchChatDownloader.Logging;
using TwitchChatDownloader.Models;

namespace TwitchChatDownloader.Writers.SrtFile
{
    internal class LoggingSrtFileWriter : ISrtFileWriter
    {
        private readonly ISrtFileWriter _srtFileWriter;
        private readonly ILog<LoggingSrtFileWriter> _log;

        public LoggingSrtFileWriter(ISrtFileWriter srtFileWriter, ILog<LoggingSrtFileWriter> log)
        {
            _srtFileWriter = srtFileWriter ?? throw new ArgumentNullException(nameof(srtFileWriter));
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public async Task<FileInfo> Write(InternalVideo internalVideo, string content)
        {
            var fileInfo = await _srtFileWriter.Write(internalVideo, content);
            _log.Info($"Wrote SRT file to {fileInfo.FullName}.");
            return fileInfo;
        }
    }
}
