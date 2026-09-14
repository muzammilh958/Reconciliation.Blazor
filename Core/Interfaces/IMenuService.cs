namespace Reconciliation.Blazor;

public interface IMenuService
{
    Task<List<MenuItemDto>> GetAllAsync();
    Task<List<RoleDto>> GetAllRolesAsync();
    Task<List<MenuPermissionDto>> GetRoleMenuPermissionsAsync(string SelectedRoleId);
    Task<List<UserDto>> GetAllUsersAsync();
    Task<SetPermissionsResponse> SetPermissionsAsync(SetPermissionsRequest request);
}
