using System;
using ApiIntegrations.Models.Twitch;

namespace ConsoleDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            var c = new ReChatDownloader.ReChatDownloader();
            //c.StoreAllVideosWithChat(new Channel {name = "zfg1"});
            var v = c.GenerateSubtitlesForAllStoredVideos();
            Console.ReadLine();

        }
    }
}
