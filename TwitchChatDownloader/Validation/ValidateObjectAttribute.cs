using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace TwitchChatDownloader.Validation
{
    public class ValidateObjectAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Encountered null object.", new[] {validationContext.MemberName});
            }

            var results = new List<ValidationResult>();
            if (value is IEnumerable enumerable)
            {
                foreach (var item in enumerable)
                {
                    if (item == null)
                    {
                        results.Add(new ValidationResult("Encountered null object.",
                            new[] {validationContext.MemberName}));
                        continue;
                    }

                    var itemContext = new ValidationContext(item, null, null);
                    Validator.TryValidateObject(item, itemContext, results, true);
                }
            }

            var context = new ValidationContext(value, null, null);
            Validator.TryValidateObject(value, context, results, true);
            return results.Any()
                ? new ValidationResult(string.Join(',', results.Select(result => result.ErrorMessage)),
                    results.SelectMany(result => result.MemberNames))
                : ValidationResult.Success;
        }
    }
}