namespace TwitchChatDownloader.Logging
{
    public interface ILog<T>
    {
        void Info(string info);
        void Warn(string warn);
        void Error(string error);
    }
}
