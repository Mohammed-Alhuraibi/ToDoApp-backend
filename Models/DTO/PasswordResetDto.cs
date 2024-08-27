namespace Models.DTO
{
    public class PasswordResetDto
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string ResetCode { get; set; }
    }
}
