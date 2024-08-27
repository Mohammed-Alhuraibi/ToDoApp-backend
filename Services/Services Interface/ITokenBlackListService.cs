namespace ToDo.Services
{
    public interface ITokenBlacklistService
    {
        void BlacklistToken(string token);
        bool IsTokenBlacklisted(string token);
        bool IsUserTokenBlacklisted(string email);
    }
}
