using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using TwitchChatDownloader.Interfaces;
using TwitchChatDownloader.Models;
using TwitchLib.Api;
using TwitchLib.Api.Core.Enums;
using TwitchLib.Api.Core.Exceptions;
using TwitchLib.Api.Helix.Models.Videos;

namespace TwitchChatDownloader.Implementations
{
    internal class VideosRetriever : IVideosRetriever
    {
        private readonly TwitchAPI _twitchApi;

        public VideosRetriever(TwitchAPI twitchApi)
        {
            _twitchApi = twitchApi;
        }

        public async Task<Result<IEnumerable<InternalVideo>>> GetVideos(string username, VideoType videoType)
        {
            try
            {
                var getUsersResponse = await _twitchApi.Helix.Users.GetUsersAsync(logins: new List<string>{username}).ConfigureAwait(false);
                var numUsers = getUsersResponse.Users.Length;
                var userId = numUsers == 1
                    ? getUsersResponse.Users.First().Id
                    : throw new Exception($"Received ${numUsers} users during GET single user request for username {username}. Was expecting 1 user. This is probably a bug.");
                var getVideosResponses = new List<GetVideosResponse>{await _twitchApi.Helix.Videos.GetVideoAsync(userId: userId, type: videoType).ConfigureAwait(false) };
                while (getVideosResponses.Last().Pagination.Cursor != null)
                {
                    getVideosResponses.Add(await _twitchApi.Helix.Videos.GetVideoAsync(userId: userId, type: videoType, after:getVideosResponses.Last().Pagination.Cursor).ConfigureAwait(false));
                }
                return Result.Ok(getVideosResponses.SelectMany(response => response.Videos).Select(video => new InternalVideo(int.Parse(video.Id), video.Title)));
            }
            catch (BadResourceException e)
            {
                return Result.Fail<IEnumerable<InternalVideo>>($"Failed to get videos for username {username}. Exception from Twitch API: {e}");
            }
        }
    }
}

