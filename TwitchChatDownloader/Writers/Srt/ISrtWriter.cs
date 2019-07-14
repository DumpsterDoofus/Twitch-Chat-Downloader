using System.Collections.Generic;
using System.Threading.Tasks;
using TwitchChatDownloader.Models;

namespace TwitchChatDownloader.Writers.Srt
{
    internal interface ISrtWriter
    {
        Task Write(InternalVideo video, IEnumerable<Comment> comments);
    }
}