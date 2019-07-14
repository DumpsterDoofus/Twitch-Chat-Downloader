using System.Collections.Generic;
using TwitchChatDownloader.Models;

namespace TwitchChatDownloader.Writers.SrtLines
{
    internal interface ICommentsParser
    {
        IEnumerable<SrtLine> Parse(IEnumerable<Comment> comments);
    }
}
