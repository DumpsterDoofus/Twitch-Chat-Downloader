using System;
using System.Collections.Generic;
using System.Linq;
using TwitchChatDownloader.Interfaces;
using TwitchChatDownloader.Models;

namespace TwitchChatDownloader.Implementations
{
    public class SrtLineWriter : ISrtLineWriter
    {
        private readonly ISrtSettings _srtSettings;

        public SrtLineWriter(ISrtSettings srtSettings)
        {
            _srtSettings = srtSettings;
        }

        public IEnumerable<SrtLine> Write(IEnumerable<InternalComment> comments)
        {
            var commentsList = comments.OrderBy(comment => comment.Timestamp).ToList();
            var numComments = commentsList.Count;
            var srtLines = commentsList.Select((comment, i) => new SrtLine(
                comment.Timestamp, 
                i < numComments - _srtSettings.MaxMessagesOnscreen 
                    ?  Min(comment.Timestamp + _srtSettings.MaxTimeOnscreen, Max(commentsList[i + _srtSettings.MaxMessagesOnscreen].Timestamp - _srtSettings.Delta, comment.Timestamp))
                    : comment.Timestamp + _srtSettings.MaxTimeOnscreen, 
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
