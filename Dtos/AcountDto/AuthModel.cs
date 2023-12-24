namespace AldhamrimediaApi.Dtos.UserDto
{
    public class AuthModel
    {
        public string Message { get; set; } = string.Empty;
        public bool IsAuthenticated { get; set; }
        public string Email { get; set; } = string.Empty;
        public int UserId { get; set; }

        public List<string>? Role { get; set; } = new List<string>();
        public string Token { get; set; } = string.Empty;
  


    }
}
