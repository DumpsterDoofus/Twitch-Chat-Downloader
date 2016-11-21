using System;
using System.IO;
using System.Linq;
using System.Text;
using Raven.Abstractions.Data;

namespace ReChatDownloader
{
    public static class ReChatDownloader
    {
        private static readonly RavenDatabaseClient RavenDatabaseClient = new RavenDatabaseClient();

        public static bool TryDownloadVideoChatToRaven(int videoId)
        {
            var v = RavenDatabaseClient.GetVideo(videoId);
            if (v != null)
            {
                return true;
            }
            try
            {
                v = new Video(videoId);
                RavenDatabaseClient.StoreVideo(v);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool GenerateSubtitlesForAllVideos()
        {
            var videos = RavenDatabaseClient.GetVideos();
            return videos.Select(GenerateSubtitles).Any(b => !b);
        }

        public static bool GenerateSubtitles(Video video)
        {
            var s = new StringBuilder();
            var lineCount = 1;
            foreach (var videoReChatResponse in video.ReChatResponses)
            {
                foreach (var datum in videoReChatResponse.data)
                {
                    s.AppendLine(lineCount.ToString());
                    var t = datum.attributes.timestamp;
                    var t2 = (long)(video.Timespan.StartTime)*1000;
                    var t3 = t - t2;
                    var a = new TimeSpan(0, 0, 0, 0, (int)t3);
                    var color = string.IsNullOrWhiteSpace(datum.attributes.color) ? "#FFFFFF" : datum.attributes.color;
                    s.AppendLine(a.ToString("hh\\:mm\\:ss\\,fff") + " --> " + a.Add(new TimeSpan(0, 0, 0, 5)).ToString("hh\\:mm\\:ss\\,fff"));
                    s.AppendLine($"<font color=\"{color}\">{datum.attributes.tags.displayname}</font>: {datum.attributes.message}");
                    s.AppendLine();
                    lineCount++;
                }
            }
            File.AppendAllText($"{video.Id}.srt", s.ToString());
            return true;
        }

        public static bool GenerateSubtitles(int videoId)
        {
            var v = RavenDatabaseClient.GetVideo(videoId);
            return v != null && GenerateSubtitles(v);
        }
    }
}
