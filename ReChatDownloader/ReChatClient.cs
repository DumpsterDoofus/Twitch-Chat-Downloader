using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace ReChatDownloader
{
    public class ReChatClient
    {
        private readonly HttpClient _client;
        private int secondsPerInterval = 30;

        public ReChatClient()
        {
            _client = new HttpClient {BaseAddress = new Uri("https://rechat.twitch.tv")};
        }

        private string GetResponseString(int videoId, int timestamp)
        {
            return _client.GetAsync($"/rechat-messages?start={timestamp}&video_id=v{videoId}").Result.Content.ReadAsStringAsync().Result;
        }

        public ReChatResponse Get(int videoId, int timestamp)
        {
            return JsonConvert.DeserializeObject<ReChatResponse>(GetResponseString(videoId, timestamp));
        }

        public VideoTimespan GetTimespan(int videoId)
        {
            // {"errors":[{"status":400,"detail":"0 is not between 1474669414 and 1474686946"}]}
            var errorResponse = GetResponseString(videoId, 0);
            var i = new Regex("\\d{10}").Matches(errorResponse);
            if (i.Count != 2)
            {
                throw new Exception("Failed to parse timestamps from ReChat error response.");
            }
            var startTime = int.Parse(i[0].Value);
            var endTime = int.Parse(i[1].Value);
            return new VideoTimespan(startTime, endTime);
        }

        public List<ReChatResponse> GetChatMessages(Video video)
        {
            var list = new List<ReChatResponse>();
            for (int i = 0; i <= video.Timespan.LengthInSeconds/secondsPerInterval; i++)
            {
                list.Add(Get(video.Id, video.Timespan.StartTime + i*secondsPerInterval));
            }
            return list;
        }

        //https://api.twitch.tv/kraken/channels/zfg1/videos?limit=60&offset=0&broadcast_type=archive&sort=time&on_site=1
    }
}