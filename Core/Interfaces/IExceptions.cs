namespace Reconciliation.Blazor;

public interface IExceptions
{
    Task<RecReconciliationExceptionDTO> GetExceptionsAsync(int batchId, int pageNumber, int pageSize, string searchValue = null,
    string sortColumn = null, string sortDirection = "desc");
}
