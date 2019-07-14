using System.Threading.Tasks;
using TwitchChatDownloader.Models;

namespace TwitchChatDownloader.Writers.Video
{
    public interface IVideoWriter
    {
        Task Write(InternalVideo internalVideo);
    }
}