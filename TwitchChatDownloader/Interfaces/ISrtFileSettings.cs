using System.IO;

namespace TwitchChatDownloader.Interfaces
{
    interface ISrtFileSettings
    {
        DirectoryInfo OutputDirectory { get; }
    }
}
