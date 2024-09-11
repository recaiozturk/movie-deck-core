
namespace MovieBringer.Core.DTOs.UserDTO
{
    public class UserCreateDto
    {
        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public string ProfileImage { get; set; } = null!;
    }
}
