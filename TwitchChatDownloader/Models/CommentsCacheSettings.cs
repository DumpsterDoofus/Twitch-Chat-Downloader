using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using CSharpFunctionalExtensions;
using TwitchChatDownloader.Interfaces;
using TwitchChatDownloader.Validation;

namespace TwitchChatDownloader.Models
{
    public class CommentsCacheSettings : Validatable, ICommentsCacheSettings
    {
        [Required]
        [ValidDirectory]
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
