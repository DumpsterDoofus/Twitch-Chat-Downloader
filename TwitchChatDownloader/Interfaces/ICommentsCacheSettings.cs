using System.IO;

namespace TwitchChatDownloader.Interfaces
{
    internal interface ICommentsCacheSettings
    {
        DirectoryInfo CacheDirectory { get; }
    }
}
