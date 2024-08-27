namespace Models.DTO
{
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string UserName { get; internal set; }
    }
}
