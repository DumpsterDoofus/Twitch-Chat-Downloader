using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitchChatDownloader.Models;
using TwitchChatDownloader.Writers.SrtFile;
using TwitchChatDownloader.Writers.SrtLines;

namespace TwitchChatDownloader.Writers.Srt
{
    internal class SrtWriter : ISrtWriter
    {
        private readonly ICommentsParser _commentsParser;
        private readonly ISrtFileWriter _srtFileWriter;

        public SrtWriter(ICommentsParser commentsParser, ISrtFileWriter srtFileWriter)
        {
            _commentsParser = commentsParser ?? throw new ArgumentNullException(nameof(commentsParser));
            _srtFileWriter = srtFileWriter ?? throw new ArgumentNullException(nameof(srtFileWriter));
        }

        public async Task Write(InternalVideo video, IEnumerable<Comment> comments)
        {
            var srtLines = _commentsParser.Parse(comments);
            var content = string.Concat(srtLines.Select(ToSrtFileText));
            await _srtFileWriter.Write(video, content);
        }

        private static string ToSrtFileText(SrtLine srtLine, int index) =>
            $"{index + 1}\n{srtLine.StartTime:hh\\:mm\\:ss\\,fff} --> {srtLine.EndTime:hh\\:mm\\:ss\\,fff}\n{srtLine.Message}\n";
    }
}
