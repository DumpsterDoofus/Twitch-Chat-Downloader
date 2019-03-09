using CommandLine;

namespace TwitchChatDownloader.Models
{
    [Verb("video", HelpText = "Download a single video.")]
    public class VideoOptions
    {
        [Option('v', "videoid", HelpText = "The video ID (for example, https://www.twitch.tv/videos/213105685 has ID 213105685). This will download subtitles for a single video.", Required = true)]
        public int VideoId { get; set; }
    }
}
