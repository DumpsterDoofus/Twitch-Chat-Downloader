using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using TwitchChatDownloader.Interfaces;
using TwitchChatDownloader.Models;

namespace TwitchChatDownloader.Implementations
{
    class LoggingCommentsRetriever : ICommentsRetriever
    {
        private readonly ICommentsRetriever _commentsRetriever;
        private readonly ILogger _logger;

        public LoggingCommentsRetriever(ICommentsRetriever commentsRetriever, ILogger logger)
        {
            _commentsRetriever = commentsRetriever;
            _logger = logger;
        }

        public async Task<IEnumerable<InternalComment>> GetComments(InternalVideo video)
        {
            _logger.Information($"Getting comments for video {video.Name}, ID {video.Id}.");
            var comments = (await _commentsRetriever.GetComments(video)).ToList();
            if (comments.Count == 0)
            {
                _logger.Warning("No comments found for video.");
            }
            else
            {
                _logger.Information($"Got {comments.Count} comments for video.");                
            }
            return comments;
        }
    }
}
