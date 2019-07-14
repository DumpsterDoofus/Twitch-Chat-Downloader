using System.IO;

namespace TwitchChatDownloader.Readers.Comments
{
    internal interface ICommentsCacheConfig
    {
        DirectoryInfo CacheDirectory { get; }
    }
}
