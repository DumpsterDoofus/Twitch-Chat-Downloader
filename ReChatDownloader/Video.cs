using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ReChatDownloader
{
    public class Video
    {
        public int Id { get; }
        public VideoTimespan Timespan { get; }
        public List<ReChatResponse> ReChatResponses { get; }
        private static readonly ReChatClient ReChatClient = new ReChatClient();

        public Video(int videoId)
        {
            Id = videoId;
            Timespan = ReChatClient.GetTimespan(videoId);
            ReChatResponses = ReChatClient.GetChatMessages(this);
        }

        
        public Video()
        {
            Id = 0;
            Timespan = new VideoTimespan();
            ReChatResponses = new List<ReChatResponse>();
        }
    }
}
