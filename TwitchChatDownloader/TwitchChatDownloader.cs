using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Serilog;
using TwitchChatDownloader.Models;
using TwitchChatDownloader.Readers.Videos;
using TwitchChatDownloader.Writers.Video;

namespace TwitchChatDownloader
{
    internal class TwitchChatDownloader
    {
        private readonly IVideoRetriever _videoRetriever;
        private readonly IVideosRetriever _videosRetriever;
        private readonly IVideoWriter _videoWriter;
        private readonly ILogger _logger;

        public TwitchChatDownloader(IVideoRetriever videoRetriever, IVideosRetriever videosRetriever, IVideoWriter videoWriter, ILogger logger)
        {
            _videoRetriever = videoRetriever ?? throw new ArgumentNullException(nameof(videoRetriever));
            _videosRetriever = videosRetriever ?? throw new ArgumentNullException(nameof(videosRetriever));
            _videoWriter = videoWriter ?? throw new ArgumentNullException(nameof(videoWriter));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Process(VideoOptions options) => 
            await _videoRetriever.GetVideo(options.VideoId)
                .Tap(internalVideo => _videoWriter.Write(internalVideo));

        public async Task Process(UserOptions options) => 
            await _videosRetriever.GetVideos(options.Username, options.VideoType)
                .Tap(async internalVideos =>
                {
                    for (var i = 0; i < internalVideos.Count; i++)
                    {
                        _logger.Information($"Getting video {i + 1} of {internalVideos.Count}.");
                        await _videoWriter.Write(internalVideos[i]);
                    }
                });
    }
}
