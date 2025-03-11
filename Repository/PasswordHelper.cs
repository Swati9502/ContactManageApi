namespace ContactManagementApi.Repository
{
    public class PasswordHelper
    {
        public static async Task<string> HashPasswordAsync(string password)
        {
            return await Task.Run(() => BCrypt.Net.BCrypt.HashPassword(password));
        }

        public static bool VerifyPassword(string? password, string? hashPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashPassword);
        }
    }
}