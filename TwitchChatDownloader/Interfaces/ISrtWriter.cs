using System.Collections.Generic;
using System.Threading.Tasks;
using TwitchChatDownloader.Models;

namespace TwitchChatDownloader.Interfaces
{
    internal interface ISrtWriter
    {
        Task Write(InternalVideo video, IEnumerable<InternalComment> comments);
    }
}