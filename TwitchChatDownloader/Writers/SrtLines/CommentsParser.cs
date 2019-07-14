using System;
using System.Collections.Generic;
using System.Linq;
using TwitchChatDownloader.Models;

namespace TwitchChatDownloader.Writers.SrtLines
{
    internal class CommentsParser : ICommentsParser
    {
        private readonly ISrtConfig _srtConfig;

        public CommentsParser(ISrtConfig srtConfig) =>
            _srtConfig = srtConfig ?? throw new ArgumentNullException(nameof(srtConfig));

        public IEnumerable<SrtLine> Parse(IEnumerable<Comment> comments)
        {
            var commentsList = comments.OrderBy(comment => comment.Timestamp).ToList();
            var numComments = commentsList.Count;
            var srtLines = commentsList.Select((comment, i) => new SrtLine(
                comment.Timestamp, 
                i < numComments - _srtConfig.MaxMessagesOnscreen 
                    ? Min(comment.Timestamp + _srtConfig.MaxTimeOnscreen, Max(commentsList[i + _srtConfig.MaxMessagesOnscreen].Timestamp - _srtConfig.Delta, comment.Timestamp))
                    : comment.Timestamp + _srtConfig.MaxTimeOnscreen, 
                $"<font color=\"{comment.Color}\">{comment.Name}</font>: {comment.Message}\n"));
            return srtLines;
        }

        private static TimeSpan Min(TimeSpan timeSpan1, TimeSpan timeSpan2) => 
            timeSpan1 < timeSpan2 
                ? timeSpan1 
                : timeSpan2;

        private static TimeSpan Max(TimeSpan timeSpan1, TimeSpan timeSpan2) => 
            timeSpan1 > timeSpan2 
                ? timeSpan1 
                : timeSpan2;
    }
}
