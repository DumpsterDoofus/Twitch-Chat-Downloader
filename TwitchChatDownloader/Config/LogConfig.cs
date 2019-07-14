using TwitchChatDownloader.Logging;
using TwitchChatDownloader.Validation;

namespace TwitchChatDownloader.Config
{
    public class LogConfig : Validatable, ILogConfig
    {
        [ValidateDirectory]
        public string LogDirectoryPath { get; set; }
    }
}
