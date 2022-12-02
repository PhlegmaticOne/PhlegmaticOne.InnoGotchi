namespace PhlegmaticOne.OperationResults.Mediatr;

public abstract class IdentityOperationResultCommandBase : IOperationResultCommand
{
    protected IdentityOperationResultCommandBase(Guid profileId)
    {
        ProfileId = profileId;
    }

    public Guid ProfileId { get; }
}