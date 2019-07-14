using System;
using System.Threading.Tasks;
using TwitchChatDownloader.Models;
using TwitchChatDownloader.Readers.Comments;
using TwitchChatDownloader.Writers.Srt;

namespace TwitchChatDownloader.Writers.Video
{
    internal class VideoWriter : IVideoWriter
    {
        private readonly ICommentsRetriever _commentsRetriever;
        private readonly ISrtWriter _srtWriter;

        public VideoWriter(ICommentsRetriever commentsRetriever, ISrtWriter srtWriter)
        {
            _commentsRetriever = commentsRetriever ?? throw new ArgumentNullException(nameof(commentsRetriever));
            _srtWriter = srtWriter ?? throw new ArgumentNullException(nameof(srtWriter));
        }

        public async Task Write(InternalVideo internalVideo)
        {
            var comments = await _commentsRetriever.GetComments(internalVideo);
            await _srtWriter.Write(internalVideo, comments);
        }
    }
}
