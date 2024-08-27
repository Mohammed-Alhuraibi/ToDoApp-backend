using System.Threading.Tasks;
using Models.DTO;

namespace ToDo.Services
{
    public interface IPasswordResetService
    {
        Task<bool> RequestPasswordReset(string email);
        Task<bool> ResendResetCode(string email);
        Task<bool> ResetPassword(PasswordResetDto resetDto);
    }
}
