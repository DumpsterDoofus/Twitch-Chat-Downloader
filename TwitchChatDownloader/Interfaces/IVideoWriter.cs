using System.Threading.Tasks;
using TwitchChatDownloader.Models;

namespace TwitchChatDownloader.Interfaces
{
    public interface IVideoWriter
    {
        Task Write(InternalVideo internalVideo);
    }
}