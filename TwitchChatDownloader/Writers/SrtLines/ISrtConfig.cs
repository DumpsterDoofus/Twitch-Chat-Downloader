using System;

namespace TwitchChatDownloader.Writers.SrtLines
{
    public interface ISrtConfig
    {
        int MaxMessagesOnscreen { get; }
        TimeSpan MaxTimeOnscreen { get; }
        TimeSpan Delta { get; }
    }
}