using CSharpFunctionalExtensions;

namespace TwitchChatDownloader.Validation
{
    public interface IValidatable
    {
        Result Validate();
    }
}
