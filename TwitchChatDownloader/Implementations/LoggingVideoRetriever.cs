using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Serilog;
using TwitchChatDownloader.Interfaces;
using TwitchChatDownloader.Models;

namespace TwitchChatDownloader.Implementations
{
    internal class LoggingVideoRetriever : IVideoRetriever
    {
        private readonly IVideoRetriever _videoRetriever;
        private readonly ILogger _logger;

        public LoggingVideoRetriever(IVideoRetriever videoRetriever, ILogger logger)
        {
            _videoRetriever = videoRetriever;
            _logger = logger;
        }

        public Task<Result<InternalVideo>> GetVideo(int videoId)
        {
            _logger.Information($"Getting info for video ID: {videoId}");
            return _videoRetriever.GetVideo(videoId)
                .OnSuccess(internalVideo => _logger.Information($"Got info for video \"{internalVideo.Name}\""))
                .OnFailure(error => _logger.Error($"Failed to get info for video ID {videoId}: {error}"));
        }
    }
}
