using System;

namespace ConsoleDownloader
{
    class Program
    {
        static void Main(string[] args)
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
                Console.WriteLine("Help info");
            }
        }

        static void Process(string path, InputType inputType, OutputType outputType)
        {
            var chatReader = new ChatReader(inputType);
            var chatWriter = new ChatWriter(outputType);
            var chatMessages = chatReader.GetChatMessages(path);
            chatWriter.WriteMessages(chatMessages);
        }
    }
}
