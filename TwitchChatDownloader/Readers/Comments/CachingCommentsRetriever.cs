using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TwitchChatDownloader.Models;

namespace TwitchChatDownloader.Readers.Comments
{
    internal class CachingCommentsRetriever : ICommentsRetriever
    {
        private readonly ICommentsCacheConfig _commentsCacheConfig;
        private readonly ICommentsRetriever _commentsRetriever;

        public CachingCommentsRetriever(ICommentsCacheConfig commentsCacheConfig, ICommentsRetriever commentsRetriever)
        {
            _commentsCacheConfig = commentsCacheConfig ?? throw new ArgumentNullException(nameof(commentsCacheConfig));
            _commentsRetriever = commentsRetriever ?? throw new ArgumentNullException(nameof(commentsRetriever));
        }

        public async Task<IEnumerable<Comment>> GetComments(InternalVideo video)
        {
            var cachePath = GetCachePath(video);
            if (File.Exists(cachePath))
            {
                var text = await File.ReadAllTextAsync(cachePath);
                return JsonConvert.DeserializeObject<IEnumerable<Comment>>(text);
            }

            var comments = await _commentsRetriever.GetComments(video);
            await File.WriteAllTextAsync(cachePath, JsonConvert.SerializeObject(comments));
            return comments;
        }

        private string GetCachePath(InternalVideo internalVideo) => 
            Path.Combine(_commentsCacheConfig.CacheDirectory.FullName, $"{internalVideo.Id}.json");
    }
}
