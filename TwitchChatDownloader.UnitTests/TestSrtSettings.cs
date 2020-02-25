using System;
using TwitchChatDownloader.Interfaces;

namespace TwitchChatDownloader.UnitTests
{
    internal class TestSrtSettings : ISrtSettings
    {
        public int MaxMessagesOnscreen { get; set; }
        public TimeSpan MaxTimeOnscreen { get; set; }
        public TimeSpan Delta { get; set; }
    }
}
