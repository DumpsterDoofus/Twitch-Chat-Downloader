using System;
using System.Threading.Tasks;
using Serilog;
using TwitchChatDownloader.Interfaces;
using TwitchChatDownloader.Models;

namespace TwitchChatDownloader.Implementations
{
    class LoggingVideoWriter : IVideoWriter
    {
        private readonly IVideoWriter _videoWriter;
        private readonly ILogger _logger;

        public LoggingVideoWriter(IVideoWriter videoWriter, ILogger logger)
        {
            _videoWriter = videoWriter ?? throw new ArgumentNullException(nameof(videoWriter));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Write(InternalVideo internalVideo)
        {
            _logger.Information($"Saving video {internalVideo.Name}.");
            await _videoWriter.Write(internalVideo);
            _logger.Information("Done saving video chat.");
        }
    }
}
