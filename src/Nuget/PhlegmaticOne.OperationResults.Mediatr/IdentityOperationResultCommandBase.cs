namespace PhlegmaticOne.OperationResults.Mediatr;

public abstract class IdentityOperationResultCommandBase : IOperationResultCommand, IIdentity
{
    protected IdentityOperationResultCommandBase(Guid profileId)
    {
        ProfileId = profileId;
    }

    public Guid ProfileId { get; }
}