using CommandLine;
using TwitchLib.Api.Core.Enums;

namespace TwitchChatDownloader.Models
{
    [Verb("user", HelpText = "Download chat for a user's videos.")]
    public class UserOptions
    {
        [Option('u', "username", HelpText = "The username (for example, https://www.twitch.tv/zfg1 has username zfg1). Use this if you want to download subtitles for all of a user's videos of a certain type.", Required = true)]
        public string Username { get; set; }

        [Option('t', "videotype", HelpText = "The type of videos to download (only matters if a username is provided). Can be: All (default), Upload, Archive, or Highlight.", Default = VideoType.All)]
        public VideoType VideoType { get; set; }
    }
}
