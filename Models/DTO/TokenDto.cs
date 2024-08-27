namespace Models.DTO
{
    public class TokenDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }



    }
    public class RefreshTokenDto
    {
        public string RefreshToken { get; set; }
    }

    public class AccessTokenDto
    {
        public string AccessToken { get; set; }
    }
}
