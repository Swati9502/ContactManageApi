using ContactManagementApi.OutputDirectory;

namespace ContactManagementApi.Interfaces
{
    public interface IRoleRepository
    {
        bool RoleExists(int roleId);
        List<Role> GetRoleByName(List<string> roleName);
        List<Role> GetRoleById(List<int> roleId);
        List<Role> GetAllRoles();
        
    }
}