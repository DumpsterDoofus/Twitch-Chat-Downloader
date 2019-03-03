using System.Threading.Tasks;
using TwitchChatDownloader.Models;

namespace TwitchChatDownloader.Interfaces
{
    internal interface ISrtFileWriter
    {
        Task Write(InternalVideo internalVideo, string content);
    }
}
