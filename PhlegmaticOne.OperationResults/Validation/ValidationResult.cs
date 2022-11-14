namespace PhlegmaticOne.OperationResults.Validation;

[Serializable]
public class ValidationResult
{
    public static ValidationResult Success => new()
    {
        IsValid = true
    };

    public static ValidationResult Error => new()
    {
        IsValid = false
    };

    internal static ValidationResult FromFailures(IDictionary<string, string[]> failures) =>
        new()
        {
            IsValid = false,
            ValidationFailures = failures.Select(x => new ValidationError
            {
                PropertyName = x.Key,
                ErrorMessage = string.Join("\n", x.Value)
            }).ToList()
        };
    public bool IsValid { get; init; }
    public List<ValidationError> ValidationFailures { get; init; } = new();

    public ValidationResult AddError(string propertyName, string errorMessage)
    {
        ValidationFailures.Add(new ValidationError
        {
            PropertyName = propertyName,
            ErrorMessage = errorMessage
        });
        return this;
    }

    public string OnlyErrors(string separator = "\n") => 
        string.Join(separator, ValidationFailures.Select(x => x.ErrorMessage));

    public IDictionary<string, string> ToDictionary() =>
        ValidationFailures
            .GroupBy(x => x.PropertyName)
            .ToDictionary(
                x => x.Key,
                x => string.Join(Environment.NewLine, x.Select(x => x.ErrorMessage))
                );
}