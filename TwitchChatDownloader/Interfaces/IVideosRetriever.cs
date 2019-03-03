using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using TwitchChatDownloader.Models;
using TwitchLib.Api.Core.Enums;

namespace TwitchChatDownloader.Interfaces
{
    public interface IVideosRetriever
    {
        Task<Result<IEnumerable<InternalVideo>>> GetVideos(string username, VideoType videoType);
    }
}
