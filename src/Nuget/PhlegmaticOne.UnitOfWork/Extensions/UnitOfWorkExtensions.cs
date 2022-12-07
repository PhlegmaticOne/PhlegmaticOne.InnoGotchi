using PhlegmaticOne.OperationResults;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.UnitOfWork.Extensions;

public static class UnitOfWorkExtensions
{
    public static async Task<OperationResult<T>> ResultFromExecutionInTransaction<T>(
        this IUnitOfWork unitOfWork, Func<Task<T>> operation)
    {
        try
        {
            var result = await operation();
            await unitOfWork.SaveChangesAsync();
            return OperationResult.FromSuccess(result);
        }
        catch (Exception e)
        {
            return OperationResult.FromFail<T>(e.Message);
        }
    }

    public static async Task<OperationResult> ResultFromExecutionInTransaction(
        this IUnitOfWork unitOfWork, Func<Task> operation)
    {
        try
        {
            await operation();
            await unitOfWork.SaveChangesAsync();
            return OperationResult.Success;
        }
        catch (Exception e)
        {
            return OperationResult.FromFail(e.Message);
        }
    }
}