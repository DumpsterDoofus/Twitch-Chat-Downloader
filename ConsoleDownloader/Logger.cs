using log4net;

namespace TwitchChatDownloader
{
    internal static class Logger
    {
        internal static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}
