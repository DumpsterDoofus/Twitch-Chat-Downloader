using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CSharpFunctionalExtensions;

namespace TwitchChatDownloader.Validation
{
    public abstract class Validatable : IValidatable
    {
        public virtual Result Validate()
        {
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(this, new ValidationContext(this), validationResults, true);
            return validationResults.Any()
                ? Result.Fail($"Validation failed for type {GetType().FullName}. {string.Concat(validationResults.Select(result => $" Error message: {result.ErrorMessage} - Member names: {string.Join('\n', result.MemberNames)}"))}")
                : Result.Ok();
        }
    }
}
