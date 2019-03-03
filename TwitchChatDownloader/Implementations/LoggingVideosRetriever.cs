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

        public async Task<Result<IEnumerable<InternalVideo>>> GetVideos(string username, VideoType videoType)
        {
            _logger.Information($"Getting info for all videos of type {videoType} from username {username}");
            var videosResult = await _videosRetriever.GetVideos(username, videoType);
            if (videosResult.IsFailure)
            {
                _logger.Error(videosResult.Error);
            }
            else
            {
                var videos = videosResult.Value.ToList();
                _logger.Information($"Got the following {videos.Count} video names:\n{string.Join('\n', videos.Select(v => v.Name))}");
            }
            return videosResult;
        }
    }
}
