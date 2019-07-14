using System;
using System.Threading.Tasks;
using TwitchChatDownloader.Logging;
using TwitchChatDownloader.Models;

namespace TwitchChatDownloader.Writers.Video
{
    internal class LoggingVideoWriter : IVideoWriter
    {
        private readonly IVideoWriter _videoWriter;
        private readonly ILog<LoggingVideoWriter> _log;

        public LoggingVideoWriter(IVideoWriter videoWriter, ILog<LoggingVideoWriter> log)
        {
            _videoWriter = videoWriter ?? throw new ArgumentNullException(nameof(videoWriter));
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public async Task Write(InternalVideo internalVideo)
        {
            _log.Info($"Saving video {internalVideo.Name}.");
            await _videoWriter.Write(internalVideo);
            _log.Info("Done saving video chat.");
        }
    }
}
