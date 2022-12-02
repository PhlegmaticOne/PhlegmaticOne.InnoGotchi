using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.ServerRequesting.Models.Requests;

public abstract class ClientOperationResultDeleteRequest<TRequest> : ClientDeleteRequest<TRequest, OperationResult>
{
    protected ClientOperationResultDeleteRequest(TRequest requestData) : base(requestData)
    {
    }
}