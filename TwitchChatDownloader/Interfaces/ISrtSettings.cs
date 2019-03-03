using System;

namespace TwitchChatDownloader.Interfaces
{
    public interface ISrtSettings
    {
        int MaxMessagesOnscreen { get; }
        TimeSpan MaxTimeOnscreen { get; }
        TimeSpan Delta { get; }
    }
}