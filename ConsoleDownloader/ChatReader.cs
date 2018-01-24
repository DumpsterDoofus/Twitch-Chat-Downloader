using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<VideoChatHistory>> GetChatHistory(string path)
        {
            switch (_inputType)
            {
                case InputType.URL:
                    var video = await GetVideoFromUrl(path);
                    return new List<VideoChatHistory> {await GetChatFromVideo(video)};
                case InputType.JSON:
                    return new List<VideoChatHistory> { GetChatFromJsonFile(path) };
                case InputType.Highlights:
                    var highlightsVideos = await _twitchClient.GetVideosAll((await GetChannelNameFromUrl(path)).name,
                        VideoType.Highlight);
                    var highlightsChats = new List<VideoChatHistory>();
                    foreach (var v in highlightsVideos)
                    {
                        highlightsChats.Add(await GetChatFromVideo(v));
                    }
                    return highlightsChats;
                case InputType.PastBroadcasts:
                    var pastBroadcastVideos = await _twitchClient.GetVideosAll((await GetChannelNameFromUrl(path)).name);
                    var pastBroadcastChats = new List<VideoChatHistory>();
                    foreach (var v in pastBroadcastVideos)
                    {
                        pastBroadcastChats.Add(await GetChatFromVideo(v));
                    }
                    return pastBroadcastChats;
                case InputType.JSONBatch:
                    var d = new DirectoryInfo(path);
                    var files = d.GetFiles("*.json");
                    return files.Select(f => GetChatFromJsonFile(f.FullName));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async Task<Video> GetVideoFromUrl(string url)
        {
            string videoId;
            try
            {
                videoId = new Regex("/videos/\\d+").Match(url).Captures[0].Value.Replace("/videos/", "");
            }
            catch (Exception e)
            {
                Logger.Log.Error($"Error parsing video URL {url}.", e);
                throw;
            }
            return await _twitchClient.GetVideos(videoId);
        }

        private async Task<Channel> GetChannelNameFromUrl(string url)
        {
            string channelName;
            try
            {
                channelName = url.Split('/')[3];
            }
            catch (Exception e)
            {
                Logger.Log.Error($"Unable to parse channel name from URL {url}.", e);
                throw;
            }
            return await _twitchClient.GetChannels(channelName);
        }

        private async Task<IEnumerable<CommentsResponse>> GetChatFromVideo(Video video)
        {
            Logger.Log.Info($"Downloading chat for video ID {video._id}.");
            return await _twitchClient.GetCommentsAll(int.Parse(video._id));
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
