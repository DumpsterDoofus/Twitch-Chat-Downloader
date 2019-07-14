using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace TwitchChatDownloader.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ValidateDirectoryAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Encountered null object.", new []{validationContext.MemberName});
            }

            var memberNames = new[] {validationContext.MemberName};
            var type = value.GetType();
            if (type != typeof(string))
            {
                return new ValidationResult($"{nameof(ValidateDirectoryAttribute)} can only be used on string properties, but was used on a property of type {type.FullName}.", memberNames);
            }

            var path = (string) value;
            var directoryInfo = new DirectoryInfo(path);
            if (!directoryInfo.Exists)
            {
                try
                {
                    directoryInfo.Create();
                }
                catch (Exception exception)
                {
                    return new ValidationResult($"Directory at path {directoryInfo.FullName} does not exist, and was unable to create it: {exception}", memberNames);
                }
            }
            return ValidationResult.Success;
        }
    }
}
