namespace Reconciliation.Blazor;

public class DashboardResponse
{
    public bool success { get; set; }
    public string message { get; set; }
    public Dashboard data { get; set; }
    public int statusCode { get; set; }
    public object errors { get; set; }    
}

public class Dashboard
{
    
    public int batchCount { get; set; }
    public int storeCount { get; set; }
    public int paymentCount { get; set; }
    public int userCount { get; set; }

}