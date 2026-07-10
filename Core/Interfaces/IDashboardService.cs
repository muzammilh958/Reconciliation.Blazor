namespace Reconciliation.Blazor;

public interface IDashboardService
{
    Task<DashboardResponse> GetAllAsync();
}
