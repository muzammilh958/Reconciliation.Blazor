public class MenuResponseDto
{
    public bool success { get; set; }
    public string? message { get; set; }
    public List<MenuItemDto>? data { get; set; }
    public int statusCode { get; set; }
    public object? errors { get; set; }
}

public class MenuItemDto
{
    public int id { get; set; }
    public string? title { get; set; }
    public string? icon { get; set; }
    public string? sectionTitle { get; set; }
    public string? url { get; set; }   // FIX
    public List<MenuItemDto> children { get; set; } = new();
    public string? Role { get; set; } 
}
