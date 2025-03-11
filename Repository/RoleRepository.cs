using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactManagementApi.Interfaces;
using ContactManagementApi.OutputDirectory;

namespace ContactManagementApi.Repository
{
     public class RoleRepository : IRoleRepository
    {
        private readonly ContactDbContext _context;
 
        public RoleRepository(ContactDbContext context)
        {
            _context = context;
        }
 
        public bool RoleExists(int roleId)
        {
            return _context.Roles.Any(r => r.RoleId == roleId);
        }

        public List<Role> GetRoleById(List<int> roleId)
        {
            return _context.Roles.Where(r => roleId.Contains(r.RoleId)).ToList();
        }
        public List<Role> GetAllRoles()
        {
            return _context.Roles.ToList();
        }

       public List<Role> GetRoleByName(List<string> roleNames)
        {
            return _context.Roles.Where(r => roleNames.Contains(r.RoleName)).ToList();
        }
    }
}