using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using TwitchChatDownloader.Models;

namespace TwitchChatDownloader.Interfaces
{
    public interface IVideoRetriever
    {
        Task<Result<InternalVideo>> GetVideo(int videoId);
    }
}