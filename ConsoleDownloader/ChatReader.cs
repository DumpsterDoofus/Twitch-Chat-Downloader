using System;
using System.IO;
using System.Text.RegularExpressions;
using ApiIntegrations.Clients;
using ApiIntegrations.Models.Twitch;
using Newtonsoft.Json;

namespace TwitchChatDownloader
{
    class ChatReader
    {
        private readonly InputType _inputType;
        public ChatReader(InputType inputType)
        {
            _inputType = inputType;
        }

        public VideoChatHistory GetChatHistory(string path)
        {
            switch (_inputType)
            {
                    case InputType.Url:
                    var twitchClient = new TwitchClient();
                    var videoId = GetVideoIdFromUrl(path);
                    return twitchClient.GetReChatAll(videoId);
                case InputType.File:
                    var json = File.ReadAllText(path);
                    var chatMessages = JsonConvert.DeserializeObject<VideoChatHistory>(json);
                    return chatMessages;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static string GetVideoIdFromUrl(string url)
        {
            return new Regex("/v/\\d+").Match(url).Captures[0].Value.Replace("/", "");
        }
    }
}
