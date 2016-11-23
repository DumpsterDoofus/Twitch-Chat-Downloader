using System;
using System.IO;
using ApiIntegrations.Clients;
using ApiIntegrations.Models.Twitch;
using Newtonsoft.Json;

namespace ConsoleDownloader
{
    class ChatReader
    {
        private readonly InputType _inputType;
        public ChatReader(InputType inputType)
        {
            _inputType = inputType;
        }

        public VideoChatHistory GetChatMessages(string path)
        {
            switch (_inputType)
            {
                    case InputType.Url:
                    var twitchClient = new TwitchClient();
                    return twitchClient.GetReChatAll(path);
                case InputType.File:
                    var json = File.ReadAllText(path);
                    var chatMessages = JsonConvert.DeserializeObject<VideoChatHistory>(json);
                    return chatMessages;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
