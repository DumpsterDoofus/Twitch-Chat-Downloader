using System;

namespace TwitchChatDownloader.Models
{
    internal class SrtLine
    {
        public TimeSpan StartTime { get; }
        public TimeSpan EndTime { get; }
        public string Message { get; }

        public SrtLine(TimeSpan startTime, TimeSpan endTime, string message)
        {
            if (startTime > endTime)
            {
                throw new ArgumentException($"Start time {startTime} is after end time {endTime}.");
            }
            StartTime = startTime;
            EndTime = endTime;
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }
    }
}
