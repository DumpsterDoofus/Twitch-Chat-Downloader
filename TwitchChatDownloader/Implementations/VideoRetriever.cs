using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using TwitchChatDownloader.Interfaces;
using TwitchChatDownloader.Models;
using TwitchLib.Api;
using TwitchLib.Api.Core.Exceptions;

namespace TwitchChatDownloader.Implementations
{
    internal class VideoRetriever : IVideoRetriever
    {
        private readonly TwitchAPI _twitchApi;

        public VideoRetriever(TwitchAPI twitchApi)
        {
            _twitchApi = twitchApi;
        }

        public async Task<Result<InternalVideo>> GetVideo(int videoId)
        {
            try
            {
                var video = await _twitchApi.V5.Videos.GetVideoAsync(videoId.ToString());
                return Result.Ok(new InternalVideo(videoId, video.Title));
            }
            catch (BadResourceException e)
            {
                return Result.Fail<InternalVideo>($"Failed to get video info for video {videoId}. Exception from Twitch API: {e}");
            }
        }
    }
}
