namespace Reconciliation.Blazor;

public class NotificationService : INotificationService
{

    public event Action<NotificationMessage>? OnNotify;

    public void ShowSuccess(string message)
        => OnNotify?.Invoke(new NotificationMessage { Message = message, Type = NotificationType.Success });

    public void ShowError(string message)
        => OnNotify?.Invoke(new NotificationMessage { Message = message, Type = NotificationType.Error });

    public void ShowInfo(string message)
        => OnNotify?.Invoke(new NotificationMessage { Message = message, Type = NotificationType.Info });

    public void ShowWarning(string message)
        => OnNotify?.Invoke(new NotificationMessage { Message = message, Type = NotificationType.Warning });


}
