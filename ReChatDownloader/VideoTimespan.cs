using System;
using Newtonsoft.Json;

namespace ReChatDownloader
{
    public class VideoTimespan
    {
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public int LengthInSeconds => EndTime - StartTime;

        [JsonConstructor]
        public VideoTimespan(int startTime, int endTime)
        {
            if (StartTime > EndTime)
            {
                throw new Exception("Start time can't be later than end time.");
            }
            if ((EndTime - StartTime)/3600 > 24)
            {
                throw new Exception("Video timespan can't be longer than 24 hours.");
            }
            StartTime = startTime;
            EndTime = endTime;
        }

        
        public VideoTimespan()
        {
            StartTime = 0;
            EndTime = 0;
        }
    }
}
