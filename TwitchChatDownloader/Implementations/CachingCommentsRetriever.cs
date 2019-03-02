using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TwitchChatDownloader.Interfaces;
using TwitchChatDownloader.Models;

namespace TwitchChatDownloader.Implementations
{
    internal class CachingCommentsRetriever : ICommentsRetriever
    {
        private readonly ICommentsCacheSettings _commentsCacheSettings;
        private readonly ICommentsRetriever _commentsRetriever;

        public CachingCommentsRetriever(ICommentsCacheSettings commentsCacheSettings, ICommentsRetriever commentsRetriever)
        {
            _commentsCacheSettings = commentsCacheSettings ?? throw new ArgumentNullException(nameof(commentsCacheSettings));
            _commentsRetriever = commentsRetriever ?? throw new ArgumentNullException(nameof(commentsRetriever));
        }

        public async Task<IEnumerable<InternalComment>> GetComments(InternalVideo video)
        {
            var cachePath = GetCachePath(video);
            if (File.Exists(cachePath))
            {
                var text = await File.ReadAllTextAsync(cachePath);
                return JsonConvert.DeserializeObject<IEnumerable<InternalComment>>(text);
            }

            var comments = await _commentsRetriever.GetComments(video);
            await File.WriteAllTextAsync(cachePath, JsonConvert.SerializeObject(comments));
            return comments;
        }

        private string GetCachePath(InternalVideo internalVideo) => 
            Path.Combine(_commentsCacheSettings.CacheDirectory.FullName, $"{internalVideo.Id}.json");
    }
}
