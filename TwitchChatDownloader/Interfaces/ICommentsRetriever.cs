using System.Collections.Generic;
using System.Threading.Tasks;
using TwitchChatDownloader.Models;

namespace TwitchChatDownloader.Interfaces
{
    internal interface ICommentsRetriever
    {
        Task<IEnumerable<InternalComment>> GetComments(InternalVideo video);
    }
}
