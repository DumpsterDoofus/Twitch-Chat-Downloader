using System.IO;

namespace TwitchChatDownloader.Interfaces
{
    interface ICommentsCacheSettings
    {
        DirectoryInfo CacheDirectory { get; }
    }
}
