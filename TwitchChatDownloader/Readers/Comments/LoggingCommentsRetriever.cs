using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitchChatDownloader.Logging;
using TwitchChatDownloader.Models;

namespace TwitchChatDownloader.Readers.Comments
{
    internal class LoggingCommentsRetriever : ICommentsRetriever
    {
        private readonly ICommentsRetriever _commentsRetriever;
        private readonly ILog<LoggingCommentsRetriever> _log;

        public LoggingCommentsRetriever(ICommentsRetriever commentsRetriever, ILog<LoggingCommentsRetriever> log)
        {
            _commentsRetriever = commentsRetriever ?? throw new ArgumentNullException(nameof(commentsRetriever));
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public async Task<IEnumerable<Comment>> GetComments(InternalVideo video)
        {
            _log.Info($"Getting comments for video {video.Name}, ID {video.Id}.");
            var comments = (await _commentsRetriever.GetComments(video)).ToList();
            if (comments.Count == 0)
            {
                _log.Warn("No comments found for video.");
            }
            else
            {
                _log.Info($"Got {comments.Count} comments for video.");                
            }
            return comments;
        }
    }
}
