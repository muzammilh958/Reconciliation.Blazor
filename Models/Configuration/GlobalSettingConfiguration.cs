namespace Reconciliation.Blazor;

public class GlobalSettingConfiguration
{
    public bool success { get; set; }
    public string message { get; set; }
    public List<GlobalSetting> data { get; set; }
    public int statusCode { get; set; }
    public object errors { get; set; }

}

public class GlobalSetting
{
    public string settingKey { get; set; }
    public string settingValue { get; set; }
    public string description { get; set; }
    public string createdBy { get; set; }
    public string updatedBy { get; set; }
    public int id { get; set; }

}