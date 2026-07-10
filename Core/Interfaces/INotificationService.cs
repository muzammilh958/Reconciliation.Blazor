namespace Reconciliation.Blazor;

public interface INotificationService
{
    event Action<NotificationMessage>? OnNotify;

    void ShowSuccess(string message);
    void ShowError(string message);
    void ShowInfo(string message);
    void ShowWarning(string message);
}
