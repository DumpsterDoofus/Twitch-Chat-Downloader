using System.Collections.Generic;
using TwitchChatDownloader.Models;

namespace TwitchChatDownloader.Interfaces
{
    internal interface ISrtLineWriter
    {
        IEnumerable<SrtLine> Write(IEnumerable<InternalComment> comment);
    }
}
