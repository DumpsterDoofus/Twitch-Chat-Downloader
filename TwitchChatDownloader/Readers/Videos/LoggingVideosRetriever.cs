using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using TwitchChatDownloader.Logging;
using TwitchChatDownloader.Models;
using TwitchLib.Api.Core.Enums;

namespace TwitchChatDownloader.Readers.Videos
{
    internal class LoggingVideosRetriever : IVideosRetriever
    {
        private readonly IVideosRetriever _videosRetriever;
        private readonly ILog<LoggingVideosRetriever> _log;

        public LoggingVideosRetriever(IVideosRetriever videosRetriever, ILog<LoggingVideosRetriever> log)
        {
            _videosRetriever = videosRetriever ?? throw new ArgumentNullException(nameof(videosRetriever));
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public Task<Result<IEnumerable<InternalVideo>>> GetVideos(string username, VideoType videoType)
        {
            _log.Info($"Getting info for all videos of type {videoType} from username {username}");
            return _videosRetriever.GetVideos(username, videoType)
                .OnSuccess(internalVideos => _log.Info($"Got info for the following {internalVideos.Count()} videos:\n{string.Join('\n', internalVideos.Select(v => v.Name))}"))
                .OnFailure(error => _log.Error($"Failed to get video info for user {username}."));
        }
    }
}
