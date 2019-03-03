using System;

namespace TwitchChatDownloader.Models
{
    internal class InternalComment
    {
        public string Name { get; }
        public string Color { get; }
        public string Message { get; }
        public TimeSpan Timestamp { get; }

        public InternalComment(string name, string color, string message, TimeSpan timestamp)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Color = color ?? "#FFFFFF";
            Message = message ?? throw new ArgumentNullException(nameof(message));
            if (timestamp < TimeSpan.Zero)
            {
                throw new ArgumentException("Timestamp cannot be negative.");
            }
            Timestamp = timestamp;
        }
    }
}
