using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ApiIntegrations.Models.Twitch;
using Newtonsoft.Json;

namespace TwitchChatDownloader
{
    internal class ChatWriter
    {
        private readonly OutputType _outputType;
        private const int MinimumMillisecondsOnScreen = 5000;

        public ChatWriter(OutputType outputType)
        {
            _outputType = outputType;
        }

        public void WriteChatHistory(List<VideoChatHistory> chatHistory)
        {
            switch (_outputType)
            {
                    case OutputType.SRT:
                    foreach (var videoChatHistory in chatHistory)
                        WriteToSrt(videoChatHistory);
                    break;
                    case OutputType.JSON:
                    foreach (var videoChatHistory in chatHistory)
                        WriteToJson(videoChatHistory);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void WriteToSrt(VideoChatHistory videoChatHistory)
        {
            var s = new StringBuilder();
            var lineCount = 1;
            var startTimeMilliseconds = videoChatHistory.ChatTimespan.StartTimestampUnixSeconds*1000;
            var numMessages = videoChatHistory.ChatMessages.Count;
            for (var i = 0; i < numMessages; i++)
            {
                var chatMessage = videoChatHistory.ChatMessages[i];
                var nextMessage = videoChatHistory.ChatMessages[i + 1 < numMessages ? i + 1 : i];
                s.AppendLine(lineCount.ToString());
                var millisecondsIntoVideo = chatMessage.attributes.TimestampUnixMilliseconds - startTimeMilliseconds;
                var millisecondsToNextMessage = nextMessage.attributes.TimestampUnixMilliseconds -
                                                chatMessage.attributes.TimestampUnixMilliseconds;
                var delay = (int)Math.Max(millisecondsToNextMessage, MinimumMillisecondsOnScreen);
                var a = new TimeSpan(0, 0, 0, 0, (int)millisecondsIntoVideo);
                var color = chatMessage.attributes.color ?? "#FFFFFF";
                s.AppendLine(a.ToString("hh\\:mm\\:ss\\,fff") + " --> " + a.Add(new TimeSpan(0, 0, 0, 0, delay)).ToString("hh\\:mm\\:ss\\,fff"));
                s.AppendLine($"<font color=\"{color}\">{chatMessage.attributes.tags.displayname}</font>: {chatMessage.attributes.message}\n");
                lineCount++;
            }
            var filename = GetSafeFilename($"{videoChatHistory.Video.title}-{videoChatHistory.Video._id}.srt");
            WriteToRelativePath($"./SRT Files/{filename}", s.ToString());
        }

        private static void WriteToJson(VideoChatHistory videoChatHistory)
        {
            var json = JsonConvert.SerializeObject(videoChatHistory);
            var filename = GetSafeFilename($"{videoChatHistory.Video.title}-{videoChatHistory.Video._id}.json");
            WriteToRelativePath($"./JSON Files/{filename}", json);
        }

        private static void WriteToRelativePath(string filePath, string content)
        {
            var file = new FileInfo(filePath);
            file.Directory?.Create();
            File.WriteAllText(file.FullName, content);
            Logger.Log.Info($"File written to {file.FullName}");
        }

        private static string GetSafeFilename(string filename)
        {
            return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
        }
    }
}
