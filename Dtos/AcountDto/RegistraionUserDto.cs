using AldhamrimediaApi.Enums;

namespace AldhamrimediaApi.Dto.UserDto
{
    public class RegistraionUserDto
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Country { get; set; } = string.Empty;
        public string Country_code { get; set; } = string.Empty;
        public string? Password { get; set; }

        public string? ConfirmPassword { get; set; }

        public string? PhoneNumber { get; set; }
        public IFormFile? Image { get; set; } 

    }
}
