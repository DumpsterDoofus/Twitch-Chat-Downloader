using TwitchChatDownloader.Models;
using TwitchLib.Api.Core.Enums;

namespace TwitchChatDownloader.UnitTests.Builders
{
    // TODO: Maybe use this in the automated tests as scaffolding.
    class OptionsBuilder
    {
        private static readonly Options Default = new Options{VideoId = 0, Username = null, VideoType = VideoType.All};

        private OptionsBuilder WithVideoId(int videoId)
        {
            Default.VideoId = videoId;
            return this;
        }

        private OptionsBuilder WithUsername(string username)
        {
            Default.Username = username;
            return this;
        }

        private OptionsBuilder WithVideoType(VideoType videoType)
        {
            Default.VideoType = videoType;
            return this;
        }

        public Options Build()
        {
            return Default;
        }
    }
}
