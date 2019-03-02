using System;

namespace TwitchChatDownloader.Interfaces
{
    interface ISrtSettings
    {
        int MaxMessagesOnscreen { get; }
        TimeSpan MaxTimeOnscreen { get; }
        TimeSpan Delta { get; }
    }
}