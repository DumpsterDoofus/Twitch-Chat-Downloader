using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace TwitchChatDownloader.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ValidDirectoryAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var memberNames = new[] {validationContext.MemberName};
            var type = value.GetType();
            if (type != typeof(string))
            {
                return new ValidationResult($"{nameof(ValidDirectoryAttribute)} can only be used on string properties, but was used on a property of type {type.FullName}.", memberNames);
            }

            var path = (string) value;
            var directoryInfo = new DirectoryInfo(path);
            return directoryInfo.Exists
                ? ValidationResult.Success
                : new ValidationResult($"Directory at path {path} does not exist.", memberNames);
        }
    }
}
