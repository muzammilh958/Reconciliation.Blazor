



using Reconciliation.Blazor.Models.Auth;

namespace Reconciliation.Blazor;

public class AppState
{

    public AuthResponse CurrentUser { get; private set; } = new();


    public event Action? OnChange;

    public void SetUser(AuthResponse session)
    {
        Console.WriteLine("======= Login Result =======");
        Console.WriteLine($"Success : {session?.Success}");
        Console.WriteLine($"Data    : {session?.Data != null}");
        Console.WriteLine($"User    : {session?.Data?.User != null}");
        Console.WriteLine($"Email   : {session?.Data?.User?.Email}");
        Console.WriteLine("SetUser()");
        Console.WriteLine(session.Data?.User?.Email);

        CurrentUser = session;
        Console.WriteLine(CurrentUser.Data?.User?.Email);

        Notify();
    }


    public AuthResponse GetUser()
    {
        return CurrentUser;

    }


    public void Clear()
    {
        CurrentUser = new AuthResponse();
        Notify();
    }


    private void Notify() => OnChange?.Invoke();


}
