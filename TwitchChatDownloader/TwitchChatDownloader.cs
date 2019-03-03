using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using TwitchChatDownloader.Interfaces;
using TwitchChatDownloader.Models;

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

        public async Task Process(Options options)
        {
            var videos = new List<InternalVideo>();
            if (options.VideoId != 0)
            {
                (await _videoRetriever.GetVideo(options.VideoId))
                    .OnSuccess(video => videos.Add(video));
            }
            if (options.Username != null)
            {
                (await _videosRetriever.GetVideos(options.Username, options.VideoType))
                    .OnSuccess(userVideos => videos.AddRange(userVideos));
            }
            foreach (var video in videos)
            {
                await _videoWriter.Write(video);
            }
        }
    }
}
