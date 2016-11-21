using System;
using Newtonsoft.Json;
using ApiIntegrations;

namespace ConsoleDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            //var r = ReChatDownloader.ReChatDownloader.GenerateSubtitles(91001230);
            var c = new ApiIntegrations.Clients.CatApiClient();
            c.HealthCheck();
            var cars = c.GetCats(1);
            Console.ReadLine();

        }
    }
}
