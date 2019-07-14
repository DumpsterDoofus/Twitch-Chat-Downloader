using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using TwitchChatDownloader.Logging;
using TwitchChatDownloader.Models;

namespace TwitchChatDownloader.Readers.Videos
{
    internal class LoggingVideoRetriever : IVideoRetriever
    {
        private readonly IVideoRetriever _videoRetriever;
        private readonly ILog<LoggingVideoRetriever> _log;

        public LoggingVideoRetriever(IVideoRetriever videoRetriever, ILog<LoggingVideoRetriever> log)
        {
            _videoRetriever = videoRetriever ?? throw new ArgumentNullException(nameof(videoRetriever));
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public Task<Result<InternalVideo>> GetVideo(int videoId)
        {
            _log.Info($"Getting info for video ID: {videoId}");
            return _videoRetriever.GetVideo(videoId)
                .OnSuccess(internalVideo => _log.Info($"Got info for video \"{internalVideo.Name}\""))
                .OnFailure(error => _log.Error($"Failed to get info for video ID {videoId}: {error}"));
        }
    }
}
