using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ApiIntegrations.Clients;
using ApiIntegrations.Models.Twitch;
using Newtonsoft.Json;

namespace TwitchChatDownloader
{
    internal class ChatReader
    {
        private readonly InputType _inputType;
        private readonly TwitchClient _twitchClient;

        public ChatReader(InputType inputType)
        {
            _inputType = inputType;
            _twitchClient = new TwitchClient();
        }

        public List<VideoChatHistory> GetChatHistory(string path)
        {
            switch (_inputType)
            {
                case InputType.URL:
                    var videoId = GetVideoIdFromUrl(path);
                    return new List<VideoChatHistory> {GetChatFromVideo(new Video {_id = videoId})};
                case InputType.JSON:
                    return new List<VideoChatHistory> { GetChatFromJsonFile(path) };
                case InputType.Highlights:
                    return _twitchClient.GetVideosAll(GetChannelNameFromUrl(path), VideoType.Highlight).Select(GetChatFromVideo).ToList();
                case InputType.PastBroadcasts:
                    return _twitchClient.GetVideosAll(GetChannelNameFromUrl(path)).Select(GetChatFromVideo).ToList();
                case InputType.JSONBatch:
                    var d = new DirectoryInfo(path);
                    var files = d.GetFiles("*.json");
                    return files.Select(f => GetChatFromJsonFile(f.FullName)).ToList();
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

        private static string GetChannelNameFromUrl(string url)
        {
            try
            {
                return url.Split('/')[3];
            }
            catch (Exception e)
            {
                Logger.Log.Error($"Unable to parse channel name from URL {url}.", e);
                throw;
            }
        }

        private VideoChatHistory GetChatFromVideo(Video video)
        {
            Logger.Log.Info($"Downloading chat for video ID {video._id}.");
            return _twitchClient.GetReChatAll(video._id);
        }

        private VideoChatHistory GetChatFromJsonFile(string path)
        {
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
                return JsonConvert.DeserializeObject<VideoChatHistory>(json);
            }
            catch (Exception e)
            {
                Logger.Log.Error($"Unable to understand contents of file at {path}.", e);
                throw;
            }
        }
    }
}
