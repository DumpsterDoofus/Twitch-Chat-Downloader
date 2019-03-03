using System;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Configuration;
using TwitchChatDownloader.Validation;

namespace TwitchChatDownloader.Extensions
{
    public static class ConfigurationExtensions
    {
        public static TValidatable GetValidatableOrThrow<TValidatable>(this IConfiguration configuration)
            where TValidatable : IValidatable
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            var type = typeof(TValidatable);
            var validatable = configuration
                .GetSection(type.Name)
                .Get<TValidatable>();
            if (validatable == null)
            {
                throw new NullReferenceException($"Couldn't find configuration for type {type}.");
            }
            validatable.Validate()
                .OnFailure(error => throw new Exception($"Validation failed when getting configuration of type {type}: {error}"));
            return validatable;
        }
    }
}
