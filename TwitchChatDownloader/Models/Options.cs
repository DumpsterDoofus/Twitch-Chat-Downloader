using CommandLine;
using TwitchLib.Api.Core.Enums;

namespace TwitchChatDownloader.Models
{
    public class Options
    {
        [Option('v', "videoid", HelpText = "The video ID (from, for example, https://www.twitch.tv/videos/213105685). This will download subtitles for a single video.")]
        public int VideoId { get; set; }

        [Option('u', "username", HelpText = "The username (from, for example, https://www.twitch.tv/zfg1). Use this if you want to download subtitles for of a user's videos of a certain type.")]
        public string Username { get; set; }

        [Option('t', "videotype", HelpText = "The type of videos to download (if a username is provided). Can be: All (default), Upload, Archive, or Highlight.", Default = VideoType.All)]
        public VideoType VideoType { get; set; }
    }
}
