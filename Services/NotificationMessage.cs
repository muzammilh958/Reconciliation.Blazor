namespace Reconciliation.Blazor;

public enum NotificationType
{
    Success,
    Error,
    Info,
    Warning
}

public class NotificationMessage
{

    public string Message { get; set; } = "";
    public NotificationType Type { get; set; }

}
