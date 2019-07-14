using System.IO;
using TwitchChatDownloader.Readers.Comments;
using TwitchChatDownloader.Validation;

namespace TwitchChatDownloader.Config
{
    public class CommentsCacheConfig : Validatable, ICommentsCacheConfig
    {
        [ValidateDirectory]
        public string CacheDirectoryPath { get; set; }

        private DirectoryInfo _cacheDirectory;
        public DirectoryInfo CacheDirectory
        {
            get
            {
                if (_cacheDirectory == null && CacheDirectoryPath != null)
                {
                    _cacheDirectory = new DirectoryInfo(CacheDirectoryPath);
                }
                return _cacheDirectory;
            }
        }
    }
}
