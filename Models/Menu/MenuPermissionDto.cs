namespace Reconciliation.Blazor;

// Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
    public class Child
    {
        public int id { get; set; }
        public string title { get; set; }
        public string icon { get; set; }
        public string url { get; set; }
        public int parentId { get; set; }
        public int order { get; set; }
        public string sectionTitle { get; set; }
        public bool hasPermission { get; set; }
        public List<Child> children { get; set; }
    }

    public class MenuPermissionDto
    {
        public int id { get; set; }
        public string title { get; set; }
        public string icon { get; set; }
        public object url { get; set; }
        public object parentId { get; set; }
        public int order { get; set; }
        public string sectionTitle { get; set; }
        public bool hasPermission { get; set; }
        public List<MenuPermissionDto> children { get; set; } = new();
    }
public class RoleDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

public class SetPermissionsRequest
{
    public string RoleId { get; set; }
    public List<int> MenuItemIds { get; set; }
}


 public class SetPermissionsResponseResponse
    {
        public string roleId { get; set; }
        public List<int> menuItemIds { get; set; }
    }

    public class SetPermissionsResponse
    {
        public bool success { get; set; }
        public string message { get; set; }
        public SetPermissionsResponseResponse data { get; set; }
        public int statusCode { get; set; }
        public object errors { get; set; }
    }

