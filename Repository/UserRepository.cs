using ContactManagementApi.Interfaces;
using ContactManagementApi.OutputDirectory;
using Microsoft.EntityFrameworkCore;

namespace ContactManagementApi.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ContactDbContext _context;

        public UserRepository(ContactDbContext context)
        {
            _context = context;
        }

        public bool UserExists(string username, string email)
        {
            return _context.Users.Any(u => u.Username == username || u.Email == email);
        }

        public User? GetUserByUsernameOrEmail(string identifier)
        {
            return _context.Users.Include(u => u.Roles).FirstOrDefault(u => u.Username == identifier || u.Email == identifier);
        }

        public void AddUser(User user)
        {
            // var roleExists= _context.Roles.Any(r =>r.RoleId==user.RoleId);
            // if(!roleExists)
            // {
            //     throw new Exception("Invalid RoleId");
            // }
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public User? GetUserByUserId(int userId)
        {
            return _context.Users.Include(u => u.Roles).FirstOrDefault(u => u.UserId == userId);
        }

        public List<string> GetUserRole(int userId)
        {
            var user = _context.Users.Include(u => u.Roles).FirstOrDefault(u => u.UserId == userId);
            return user?.Roles.Select(r => r.RoleName).ToList() ?? new List<string>();
        }

        public void AssignRoleToUser(int userId, int roleId)
        {
            var user=_context.Users.FirstOrDefault(u =>u.UserId == userId); 

            var role=_context.Roles.FirstOrDefault(r =>r.RoleId == roleId);
            if(user!=null && role!= null)
            {
                user.Roles.Add(role);
                _context.SaveChanges();
            }
        }

        
    }
}