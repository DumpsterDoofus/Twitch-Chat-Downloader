using System.IO;
using System.Threading.Tasks;
using TwitchChatDownloader.Models;

namespace TwitchChatDownloader.Writers.SrtFile
{
    internal interface ISrtFileWriter
    {
        Task<FileInfo> Write(InternalVideo internalVideo, string content);
    }
}
