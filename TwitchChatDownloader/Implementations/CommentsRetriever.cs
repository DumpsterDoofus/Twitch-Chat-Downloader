using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitchChatDownloader.Interfaces;
using TwitchChatDownloader.Models;
using TwitchLib.Api;

namespace TwitchChatDownloader.Implementations
{
    internal class CommentsRetriever : ICommentsRetriever
    {
        private readonly TwitchAPI _twitchApi;

        public CommentsRetriever(TwitchAPI twitchApi)
        {
            _twitchApi = twitchApi ?? throw new ArgumentNullException(nameof(twitchApi));
        }

        public async Task<IEnumerable<InternalComment>> GetComments(InternalVideo video)
        {
            var comments = await _twitchApi.Undocumented.GetAllCommentsAsync(video.Id.ToString()).ConfigureAwait(false);
            return comments
                .SelectMany(page => page.Comments)
                .Select(comment =>
                new InternalComment(
                    comment.Commenter.DisplayName ?? comment.Commenter.Name, 
                    comment.Message.UserColor,
                    comment.Message.Body, 
                    TimeSpan.FromSeconds(comment.ContentOffsetSeconds)));
        }
    }
}

