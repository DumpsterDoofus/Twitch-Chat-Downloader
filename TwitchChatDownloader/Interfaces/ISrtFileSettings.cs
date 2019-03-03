using System.IO;

namespace TwitchChatDownloader.Interfaces
{
    public interface ISrtFileSettings
    {
        DirectoryInfo OutputDirectory { get; }
    }
}
