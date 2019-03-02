using System.Collections.Generic;
using TwitchChatDownloader.Models;

namespace TwitchChatDownloader.Interfaces
{
    interface ISrtLineWriter
    {
        IEnumerable<SrtLine> Write(IEnumerable<InternalComment> comment);
    }
}
