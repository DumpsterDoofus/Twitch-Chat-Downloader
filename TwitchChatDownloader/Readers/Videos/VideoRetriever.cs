using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using TwitchChatDownloader.Models;
using TwitchLib.Api;
using TwitchLib.Api.Core.Exceptions;

namespace TwitchChatDownloader.Readers.Videos
{
    internal class VideoRetriever : IVideoRetriever
    {
        private readonly TwitchAPI _twitchApi;

        public VideoRetriever(TwitchAPI twitchApi) =>
            _twitchApi = twitchApi ?? throw new ArgumentNullException(nameof(twitchApi));

        public async Task<Result<InternalVideo>> GetVideo(int videoId)
        {
            try
            {
                var videos = (await _twitchApi.Helix.Videos.GetVideoAsync(new List<string> {videoId.ToString()})).Videos;
                var numVideos = videos.Length;
                if (numVideos != 1)
                {
                    throw new Exception($"Received ${numVideos} videos during GET single video request. Was expecting 1 video. This is probably a bug.");
                }
                var video = videos.First();
                return Result.Ok(new InternalVideo(int.Parse(video.Id), video.Title));
            }
            catch (BadResourceException badResourceException)
            {
                return Result.Fail<InternalVideo>($"Failed to get video info for video {videoId}. Exception from Twitch API: {badResourceException}");
            }
        }
    }
}
