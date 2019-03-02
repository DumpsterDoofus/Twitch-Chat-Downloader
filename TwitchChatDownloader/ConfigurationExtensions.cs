using System.Linq;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Configuration;
using TwitchChatDownloader.Validation;

namespace TwitchChatDownloader
{
    public static class ConfigurationExtensions
    {
        public static Result GetValidatable<T>(this IConfiguration configuration, out T t) where T : IValidatable
        {
            t = configuration.Get<T>();
            return t.Validate();
        }
    }
}
