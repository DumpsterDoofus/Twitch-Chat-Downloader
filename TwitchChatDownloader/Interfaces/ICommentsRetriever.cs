using System.Collections.Generic;
using System.Threading.Tasks;
using TwitchChatDownloader.Models;

namespace TwitchChatDownloader.Interfaces
{
    interface ICommentsRetriever
    {
        Task<IEnumerable<InternalComment>> GetComments(InternalVideo video);
    }
}
