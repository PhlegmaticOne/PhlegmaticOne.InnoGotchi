namespace PhlegmaticOne.OperationResults.Mediatr;

public abstract class IdentityOperationResultQueryBase<T> : IOperationResultQuery<T>, IIdentity
{
    protected IdentityOperationResultQueryBase(Guid profileId)
    {
        ProfileId = profileId;
    }

    public Guid ProfileId { get; }
}