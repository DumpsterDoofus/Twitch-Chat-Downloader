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
                case InputType.URL:
                    var twitchClient = new TwitchClient();
                    var videoId = GetVideoIdFromUrl(path);
                    return twitchClient.GetReChatAll(videoId);
                case InputType.JSON:
                    string json;
                    try
                    {
                        json = File.ReadAllText(path);
                    }
                    catch (Exception e)
                    {
                        Logger.Log.Error($"Unable to open file at {path}.", e);
                        throw;
                    }
                    try
                    {
                        var chatMessages = JsonConvert.DeserializeObject<VideoChatHistory>(json);
                        return chatMessages;

                    }
                    catch (Exception e)
                    {
                        Logger.Log.Error($"Unable to understand contents of file at {path}.", e);
                        throw;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static string GetVideoIdFromUrl(string url)
        {
            try
            {
                return new Regex("/v/\\d+").Match(url).Captures[0].Value.Replace("/", "");
            }
            catch (Exception e)
            {
                Logger.Log.Error($"Error parsing video URL {url}.", e);
                throw;
            }
        }
    }
}
