using System;
using System.IO;
using System.Linq;
using System.Text;
using ApiIntegrations.Clients;
using ApiIntegrations.Models.Twitch;
using Raven.Abstractions.Data;

namespace ReChatDownloader
{
    public class ReChatDownloader
    {
        private readonly RavenDatabaseClient RavenDatabaseClient;
        private readonly TwitchClient TwitchClient;

        public ReChatDownloader()
        {
            RavenDatabaseClient = new RavenDatabaseClient();
            TwitchClient = new TwitchClient();
        }

        public void StoreAllVideosWithChat(Channel channel, VideoType videoType = VideoType.PastBroadcast)
        {
            var videos = TwitchClient.GetVideosAll(channel, videoType);
            foreach (var video in videos)
            {
                RavenDatabaseClient.StoreVideo(TwitchClient.GetReChatAll(video));
            }
        }

        public bool GenerateSubtitlesForAllStoredVideos()
        {
            var videos = RavenDatabaseClient.Get<VideoWithChat>();
            return videos.Select(GenerateSubtitles).Any(b => !b);
        }

        public bool GenerateSubtitles(VideoWithChat video)
        {
            var s = new StringBuilder();
            var lineCount = 1;
            foreach (var videoReChatResponse in video.ReChatResponses)
            {
                foreach (var datum in videoReChatResponse.data)
                {
                    s.AppendLine(lineCount.ToString());
                    var t = datum.attributes.timestamp;
                    var t2 = video.StartTimestamp * 1000;
                    var t3 = t - t2;
                    var a = new TimeSpan(0, 0, 0, 0, (int)t3);
                    var color = string.IsNullOrWhiteSpace(datum.attributes.color) ? "#FFFFFF" : datum.attributes.color;
                    s.AppendLine(a.ToString("hh\\:mm\\:ss\\,fff") + " --> " + a.Add(new TimeSpan(0, 0, 0, 5)).ToString("hh\\:mm\\:ss\\,fff"));
                    s.AppendLine($"<font color=\"{color}\">{datum.attributes.tags.displayname}</font>: {datum.attributes.message}");
                    s.AppendLine();
                    lineCount++;
                }
            }
            var filename = GetSafeFilename($"{video.Video.title}-{video.Video._id}.srt");
            File.AppendAllText(filename, s.ToString());
            return true;
        }

        public string GetSafeFilename(string filename)
        {

            return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));

        }
    }
}
