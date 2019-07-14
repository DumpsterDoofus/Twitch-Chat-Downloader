using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using TwitchChatDownloader.Models;

namespace TwitchChatDownloader.Readers.Videos
{
    public interface IVideoRetriever
    {
        Task<Result<InternalVideo>> GetVideo(int videoId);
    }
}