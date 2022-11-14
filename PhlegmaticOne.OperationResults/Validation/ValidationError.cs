namespace PhlegmaticOne.OperationResults.Validation;

[Serializable]
public class ValidationError
{
    public string PropertyName { get; init; } = null!;
    public string ErrorMessage { get; init; } = null!;
}