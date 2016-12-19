using System;
using System.Threading.Tasks;

namespace TwitchChatDownloader
{
    internal static class Program
    {
        private const string HelpInstructions = @"Usage is TwitchChatDownloader.exe [flags]

Flags (case-insensitive):
-path (required): Either a URL of a Twitch video or channel, or a physical path (can be either relative, like JSON Files or fully-qualified like C:\Users\Peter\Documents\Visual Studio 2015\Projects\ReChat\ConsoleDownloader\bin\Debug\JSON Files).
-inputtype: How the path gets processed.
 - url (default): Downloads a single video at the specified URL.
 - pastbroadcasts: Downloads all past broadcasts of the channel at the specified URL.
 - highlights: Downloads all highlights of the channel at the specified URL.
 - json: Processes a JSON file.
 - jsonbatch: Processes all JSON files in the specified directory.
-outputtype: How the chat gets saved.
 - srt (default): Messages are saved to an SRT file, which can be used as a subtitle track on video players. Messages stay onscreen for either 5 seconds or the time until the next message, whichever is longer. Usernames are colored with the same color as they do in chat.
 - json: Saves raw traffic in original form as received from Twitch's API. This is useful in the event I change the behavior of the SRT save process, since saving as SRT is ""lossy"" from an information standpoint.

Examples:

## A single video, saving chat as SRT
TwitchChatDownloader -path https://www.twitch.tv/zfg1/v/69027652 -inputtype url -outputtype srt
TwitchChatDownloader -path https://www.twitch.tv/zfg1/v/69027652 (behaves same as previous)

## A single video, saving chat as JSON
TwitchChatDownloader -path https://www.twitch.tv/zfg1/v/69027652 -inputtype url -outputtype json

## Converting a JSON file to SRT
TwitchChatDownloader -path ""JSON Files/JsonFromExample3.json"" -inputtype json -outputtype srt

## Saving all video highlights from a channel as SRT
TwitchChatDownloader -path https://www.twitch.tv/zfg1 -inputtype highlights -outputtype srt

## Saving all past broadcasts from a channel as JSON
TwitchChatDownloader -path https://www.twitch.tv/zfg1 -inputtype pastbroadcasts -outputtype json

## Converting all JSON files in a folder to SRT
TwitchChatDownloader -path ""JSON Files"" -inputtype json -outputtype jsonbatch";

        private static void Main(string[] args)
        {
            var arguments = new Arguments(args);
            InputType inputType;
            OutputType outputType;
            var bInputType = Enum.TryParse(arguments["inputtype"] ?? "url", true, out inputType);
            var bOutputType = Enum.TryParse(arguments["outputtype"] ?? "srt", true, out outputType);
            var path = arguments["path"];
            if (path != null)
            {
                if (bInputType)
                {
                    if (bOutputType)
                    {
                        try
                        {
                            Process(path, inputType, outputType).Wait();
                        }
                        catch (Exception)
                        {
                            // ignored, will be logged.
                        }
                    }
                    else
                    {
                        Logger.Log.Error("Invalid outputtype.");
                    }
                }
                else
                {
                    Logger.Log.Error("Invalid inputtype.");
                }
            }
            else
            {
                Logger.Log.Info(HelpInstructions);
            }
        }

        private static async Task Process(string path, InputType inputType, OutputType outputType)
        {
            var chatReader = new ChatReader(inputType);
            var chatWriter = new ChatWriter(outputType);
            var chatHistory = await chatReader.GetChatHistory(path);
            chatWriter.WriteChatHistory(chatHistory);
        }
    }
}
