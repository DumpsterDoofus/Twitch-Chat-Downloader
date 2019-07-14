using System;
using System.ComponentModel.DataAnnotations;
using TwitchChatDownloader.Validation;
using TwitchChatDownloader.Writers.SrtLines;

namespace TwitchChatDownloader.Config
{
    public class SrtConfig : Validatable, ISrtConfig
    {
        [Range(1, 10)]
        public int MaxMessagesOnscreen { get; set; }

        [Range(1, 100)]
        public int MaxSecondsOnscreen { get; set; }

        [Range(0, 1000)]
        public int DeltaMilliseconds { get; set; }

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

        private TimeSpan _delta;
        public TimeSpan Delta
        {
            get
            {
                if (_delta == default)
                {
                    _delta = TimeSpan.FromMilliseconds(DeltaMilliseconds);
                }

                return _delta;
            }
        }
    }
}
