using ContactManagementApi.Dtos;
using ContactManagementApi.Interfaces;
using ContactManagementApi.OutputDirectory;
using ContactManagementApi.Repository;
using ContactManagementApi.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactManagementApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly JwtService _jwtService;

        public UserController(IUserRepository userRepository, JwtService jwtService, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _roleRepository = roleRepository;

        }

        [HttpPost("Register")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Register([FromForm] UserDto userDto)
        {
            if (string.IsNullOrWhiteSpace(userDto.Username) || string.IsNullOrWhiteSpace(userDto.Email) || string.IsNullOrWhiteSpace(userDto.Password))
                return BadRequest("All of these are required");

            if (_userRepository.UserExists(userDto.Username, userDto.Email))
                return BadRequest("Username or email already exists.");

            var roleNames = userDto.Role.Split(',').Select(r => r.Trim()).ToList();
            var roles = _roleRepository.GetRoleByName(roleNames);
            if (roles.Count == 0)
                return BadRequest("Invalid role.");


            var hashPassword = await PasswordHelper.HashPasswordAsync(userDto.Password);
            
            var user = new User
            {
                Username = userDto.Username,
                Email = userDto.Email,
                PasswordHash = hashPassword,
                ProfilePhoto = userDto.ProfilePhoto != null ? FileHelper.ConvertToByteArray(userDto.ProfilePhoto) : null,
                Roles = roles
            };
            _userRepository.AddUser(user);

            return Ok(new { message = "User registered successfully." });
        }

        [HttpGet("GetUserRoles/{userId}")]
        public IActionResult GetUserRoles(int userId)
        {
            var user = _userRepository.GetUserByUserId(userId);
            if (user == null)
                return NotFound("User not found.");

            var roles = _userRepository.GetUserRole(userId);
            if (roles.Count == 0)
                return NotFound("No roles found for this user.");

            return Ok(roles);
        }
        [HttpGet("GetAllRoles")]
        public IActionResult GetAllRoles()
        {
            var roles = _roleRepository.GetAllRoles();
            return Ok(roles);
        }


        [HttpPost("Login")]
        public IActionResult Login(LoginDto loginDto)
        {
            if (string.IsNullOrWhiteSpace(loginDto.Username) || string.IsNullOrWhiteSpace(loginDto.Password))
                return BadRequest("Username and password are required");

            var user = _userRepository.GetUserByUsernameOrEmail(loginDto.Username);
            if (user == null || !PasswordHelper.VerifyPassword(loginDto.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials.");

            var token = _jwtService.GenerateToken(user);
            var userId = user.UserId;
            var roles = user.Roles.Select(r => r.RoleName).ToList();
            return Ok(new { Token = token, UserId = userId, role = roles });
        }
        //[Authorize(Roles = "Admin")]
        [HttpGet("Profile")]
        public IActionResult GetProfileByUserId(int userId)
        {
            var user = _userRepository.GetUserByUserId(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            return Ok(new
            {
                username = user.Username,
                email = user.Email,
                profilePicture = user.ProfilePhoto,
                role = user.Roles.Select(r => r.RoleName).ToList()
            });
        }
        //[Authorize(Roles = "Admin")]
        [HttpPut("Profile")]
        public IActionResult UpdateUserProfile(int userId, [FromForm] UserProfileDto updateUserProfileDto)
        {
            var user = _userRepository.GetUserByUserId(userId);
            if (user == null)
                return NotFound(new { message = "User not found." });

            user.Username = updateUserProfileDto.Username;


            var roleNames = updateUserProfileDto.Role.Split(',').Select(r => r.Trim()).ToList();
            var roles = _roleRepository.GetRoleByName(roleNames);
            if (roles.Count > 0)
            {
                user.Roles.Clear();
                user.Roles = roles;
            }
            else
            {
                return BadRequest(new { message = "Invalid roles provided." });
            }
            if (updateUserProfileDto.ProfilePhoto != null && updateUserProfileDto.ProfilePhoto.Length > 0)
            {
                user.ProfilePhoto = FileHelper.ConvertToByteArray(updateUserProfileDto.ProfilePhoto);
            }

            _userRepository.UpdateUser(user);

            return Ok(new
            {
                message = "Profile updated successfully.",
                username = user.Username,
                ProfilePicture = user.ProfilePhoto,
                roles = user.Roles.Select(r => r.RoleName).ToList()
            });
        }
    }
}