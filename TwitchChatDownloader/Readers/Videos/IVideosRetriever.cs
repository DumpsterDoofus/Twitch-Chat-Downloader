using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using TwitchChatDownloader.Models;
using TwitchLib.Api.Core.Enums;

namespace TwitchChatDownloader.Readers.Videos
{
    public interface IVideosRetriever
    {
        Task<Result<IReadOnlyList<InternalVideo>>> GetVideos(string username, VideoType videoType);
    }
}
