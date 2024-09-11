
namespace MovieBringer.Core.DTOs.UserDTO
{
    public class UserUpdateDto
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        //public string Password { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;

    }
}
