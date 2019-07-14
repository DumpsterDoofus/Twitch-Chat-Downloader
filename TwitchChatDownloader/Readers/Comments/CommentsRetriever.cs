using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitchChatDownloader.Models;
using TwitchLib.Api;

namespace TwitchChatDownloader.Readers.Comments
{
    internal class CommentsRetriever : ICommentsRetriever
    {
        private readonly TwitchAPI _twitchApi;

        public CommentsRetriever(TwitchAPI twitchApi) =>
            _twitchApi = twitchApi ?? throw new ArgumentNullException(nameof(twitchApi));

        public async Task<IEnumerable<Comment>> GetComments(InternalVideo video)
        {
            var comments = await _twitchApi.Undocumented.GetAllCommentsAsync(video.Id.ToString());
            return comments
                .SelectMany(page => page.Comments)
                .Select(comment =>
                new Comment(
                    comment.Commenter.DisplayName ?? comment.Commenter.Name, 
                    comment.Message.UserColor,
                    comment.Message.Body, 
                    TimeSpan.FromSeconds(comment.ContentOffsetSeconds)));
        }
    }
}

