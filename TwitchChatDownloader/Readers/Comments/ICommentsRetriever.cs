using System.Collections.Generic;
using System.Threading.Tasks;
using TwitchChatDownloader.Models;

namespace TwitchChatDownloader.Readers.Comments
{
    internal interface ICommentsRetriever
    {
        Task<IReadOnlyList<Comment>> GetComments(InternalVideo video);
    }
}
