using System;
using System.IO;
using System.Text;
using ApiIntegrations.Models.Twitch;
using Newtonsoft.Json;

namespace TwitchChatDownloader
{
    class ChatWriter
    {
        private readonly OutputType _outputType;

        public ChatWriter(OutputType outputType)
        {
            _outputType = outputType;
        }

        public void WriteChatHistory(VideoChatHistory chatHistory)
        {
            switch (_outputType)
            {
                    case OutputType.SRT:
                    WriteToSrt(chatHistory);
                    break;
                    case OutputType.JSON:
                    WriteToJson(chatHistory);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void WriteToSrt(VideoChatHistory videoChatHistory)
        {
            var s = new StringBuilder();
            var lineCount = 1;
            var startTimeMilliseconds = videoChatHistory.ChatTimespan.StartTime*1000;
            foreach (var chatMessage in videoChatHistory.ChatMessages)
            {
                s.AppendLine(lineCount.ToString());
                var millisecondsIntoVideo = chatMessage.attributes.timestamp - startTimeMilliseconds;
                var a = new TimeSpan(0, 0, 0, 0, (int)millisecondsIntoVideo);
                var color = chatMessage.attributes.color ?? "#FFFFFF";
                s.AppendLine(a.ToString("hh\\:mm\\:ss\\,fff") + " --> " + a.Add(new TimeSpan(0, 0, 0, 5)).ToString("hh\\:mm\\:ss\\,fff"));
                s.AppendLine($"<font color=\"{color}\">{chatMessage.attributes.tags.displayname}</font>: {chatMessage.attributes.message}\n");
                lineCount++;
            }

            var filename = GetSafeFilename($"{videoChatHistory.Video.title}-{videoChatHistory.Video._id}.srt");
            File.AppendAllText(filename, s.ToString());
        }

        private static string GetSafeFilename(string filename)
        {
            return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
        }

        private static void WriteToJson(VideoChatHistory videoChatHistory)
        {
            var json = JsonConvert.SerializeObject(videoChatHistory);
            var filename = GetSafeFilename($"{videoChatHistory.Video.title}-{videoChatHistory.Video._id}.json");
            File.WriteAllText(filename, json);
        }
    }
}
