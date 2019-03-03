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

        public async Task<Result<InternalVideo>> GetVideo(int videoId)
        {
            _logger.Information($"Getting info for video ID: {videoId}");
            var video = await _videoRetriever.GetVideo(videoId);
            if (video.IsFailure)
            {
                _logger.Error(video.Error);
            }
            else
            {
                _logger.Information($"Got info for video \"{video.Value.Name}\"");                
            }
            return video;
        }
    }
}
