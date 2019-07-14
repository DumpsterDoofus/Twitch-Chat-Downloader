using System.Collections.Generic;
using System.Threading.Tasks;
using TwitchChatDownloader.Models;

namespace TwitchChatDownloader.Readers.Comments
{
    internal interface ICommentsRetriever
    {
        Task<IEnumerable<Comment>> GetComments(InternalVideo video);
    }
}
