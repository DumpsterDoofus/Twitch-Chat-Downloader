using System;
using System.ComponentModel.DataAnnotations;
using TwitchChatDownloader.Interfaces;
using TwitchChatDownloader.Validation;

namespace TwitchChatDownloader.Models
{
    class SrtSettings : Validatable, ISrtSettings
    {
        [Range(1, 10)]
        public int MaxMessagesOnscreen { get; private set; }

        [Range(1, 100)]
        public int MaxSecondsOnscreen { get; set; }

        private TimeSpan _maxTimeOnscreen;
        public TimeSpan MaxTimeOnscreen
        {
            get
            {
                if (_maxTimeOnscreen == default)
                {
                    _maxTimeOnscreen = TimeSpan.FromSeconds(MaxSecondsOnscreen);
                }

                return _maxTimeOnscreen;
            }
        }

        public TimeSpan Delta { get; private set; } //TODO: Delete
    }
}
