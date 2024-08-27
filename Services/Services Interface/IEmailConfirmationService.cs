using System.Threading.Tasks;
using Models.DTO;

namespace ToDo.Services.Interfaces
{
    public interface IEmailConfirmationService
    {
        Task<(bool Success, string Message)> ConfirmEmailAsync(ConfirmEmailDto confirmEmailDto);
        Task<(bool Success, string Message)> ResendConfirmationCodeAsync(ResendConfirmationCodeDto resendConfirmationCodeDto);
    }
}
