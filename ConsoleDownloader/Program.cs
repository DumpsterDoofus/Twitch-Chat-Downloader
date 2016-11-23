using System;

namespace TwitchChatDownloader
{
    internal static class Program
    {
        private const string HelpInstructions = @"Usage is TwitchChatDownloader.exe [flags]

Flags:
-path (required): Either a URL of a Twitch video or the physical path of a previously-downloaded JSON file.
-inputtype: Either ""file"" or ""url"". Defaults to ""url"".
-outputtype: Either ""srt"" or ""json"". Defaults to ""srt"".

Examples:

1. TwitchChatDownloader -path https://www.twitch.tv/zfg1/v/69027652 -inputtype url -outputtype srt
2. TwitchChatDownloader -path https://www.twitch.tv/zfg1/v/69027652 
(same as example 1)
3. TwitchChatDownloader -path https://www.twitch.tv/zfg1/v/69027652 -inputtype url -outputtype json
4. TwitchChatDownloader -path ""C:\JsonFromExample3.json"" -inputtype file -outputtype srt
";

        private static void Main(string[] args)
        {
            var arguments = new Arguments(args);
            InputType inputType;
            OutputType outputType;
            var bInputType = Enum.TryParse(arguments["inputtype"], true, out inputType);
            var bOutputType = Enum.TryParse(arguments["outputtype"], true, out outputType);
            var path = arguments["path"];
            if (path != null)
            {
                if (bInputType)
                {
                    if (bOutputType)
                    {
                        Process(path, inputType, outputType);
                    }
                    else
                    {
                        Console.WriteLine("Invalid outputtype.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid inputtype.");
                }
            }
            else
            {
                Console.WriteLine(HelpInstructions);
            }
        }

        private static void Process(string path, InputType inputType, OutputType outputType)
        {
            var chatReader = new ChatReader(inputType);
            var chatWriter = new ChatWriter(outputType);
            var chatHistory = chatReader.GetChatHistory(path);
            chatWriter.WriteChatHistory(chatHistory);
        }
    }
}
