
namespace ContactManagementApi.Dtos
{
    public class UserDto
    {
        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public IFormFile? ProfilePhoto { get; set; }

        public string Role { get; set; } = string.Empty;
        
    }

}