using System.IO;

namespace TwitchChatDownloader.Writers.SrtFile
{
    public interface ISrtFileConfig
    {
        DirectoryInfo OutputDirectory { get; }
    }
}
