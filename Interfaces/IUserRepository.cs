using ContactManagementApi.OutputDirectory;

namespace ContactManagementApi.Interfaces
{
    public interface IUserRepository
    {
        bool UserExists(string username, string email);
        void AddUser(User user);
        User? GetUserByUsernameOrEmail(string usernameOrEmail);
        void UpdateUser(User user);
        User? GetUserByUserId(int userId);
        void AssignRoleToUser(int userId,int roleId);
        List <string> GetUserRole(int userId);
       
    }
}