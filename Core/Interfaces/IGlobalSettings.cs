namespace Reconciliation.Blazor;

public interface IGlobalSettings
{
    Task<List<GlobalSetting>> GetAllAsync();

    Task<GlobalSetting> UpdateAsync(int id, GlobalSetting request);

}
