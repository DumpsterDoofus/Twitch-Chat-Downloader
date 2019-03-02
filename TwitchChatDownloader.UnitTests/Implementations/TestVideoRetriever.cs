//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;
//using TwitchChatDownloader.Interfaces;
//using TwitchLib.Enums;
//using TwitchLib.Models.API.Helix.Videos.GetVideos;

//namespace TwitchChatDownloader.UnitTests
//{
//    class TestVideoRetriever : IVideoRetriever
//    {
//        public async Task<IEnumerable<Video>> GetVideos(string username, VideoType videoType)
//        {
//            return new List<Video>()
//            {
//                new Video(){}
//            };
//        }

//        public async Task<Video> GetVideo(int videoId)
//        {
//            var video = new Video {Id = videoId.ToString()};
//            return video;
//        }
//    }
//}
