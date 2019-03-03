using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Serilog;
using TwitchChatDownloader.Interfaces;
using TwitchChatDownloader.Models;
using TwitchLib.Api.Core.Enums;

namespace TwitchChatDownloader.Implementations
{
    internal class LoggingVideosRetriever : IVideosRetriever
    {
        private readonly IVideosRetriever _videosRetriever;
        private readonly ILogger _logger;

        public LoggingVideosRetriever(IVideosRetriever videosRetriever, ILogger logger)
        {
            _videosRetriever = videosRetriever;
            _logger = logger;
        }

        public Task<Result<IEnumerable<InternalVideo>>> GetVideos(string username, VideoType videoType)
        {
            _logger.Information($"Getting info for all videos of type {videoType} from username {username}");
            return _videosRetriever.GetVideos(username, videoType)
                .OnSuccess(internalVideos => _logger.Information($"Got info for the following {internalVideos.Count()} videos:\n{string.Join('\n', internalVideos.Select(v => v.Name))}"))
                .OnFailure(error => _logger.Error($"Failed to get video info for user {username}."));
        }
    }
}
