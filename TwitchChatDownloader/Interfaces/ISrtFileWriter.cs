using System.Threading.Tasks;
using TwitchChatDownloader.Models;

namespace TwitchChatDownloader.Interfaces
{
    interface ISrtFileWriter
    {
        Task Write(InternalVideo internalVideo, string content);
    }
}
