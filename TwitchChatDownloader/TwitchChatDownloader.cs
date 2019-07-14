using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
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

        public TwitchChatDownloader(IVideoRetriever videoRetriever, IVideosRetriever videosRetriever, IVideoWriter videoWriter)
        {
            _videoRetriever = videoRetriever ?? throw new ArgumentNullException(nameof(videoRetriever));
            _videosRetriever = videosRetriever ?? throw new ArgumentNullException(nameof(videosRetriever));
            _videoWriter = videoWriter ?? throw new ArgumentNullException(nameof(videoWriter));
        }

        public async Task Process(VideoOptions options) => 
            await _videoRetriever.GetVideo(options.VideoId)
                .OnSuccess(internalVideo => _videoWriter.Write(internalVideo));

        public async Task Process(UserOptions options) => 
            await _videosRetriever.GetVideos(options.Username, options.VideoType)
                .OnSuccess(async internalVideos =>
                {
                    foreach (var video in internalVideos)
                    {
                        await _videoWriter.Write(video);
                    }
                });
    }
}
