namespace Reconciliation.Blazor;

public interface IExceptionService
{
     Task<ExceptionlistDTO> GetExceptionlist(string batchId);
}
