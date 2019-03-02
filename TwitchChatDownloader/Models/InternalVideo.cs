using System;

namespace TwitchChatDownloader.Models
{
    public class InternalVideo
    {
        public int Id { get; }
        public string Name { get; }

        public InternalVideo(int id, string name)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID of a video must be a positive integer.");
            }
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}
