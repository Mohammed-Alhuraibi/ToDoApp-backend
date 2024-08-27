using Models.DTO;
using ToDo.Models;

namespace ToDo.Services
{
    public interface IRegistrationService
    {
        Task<User> RegisterUser(UserRegistrationDto userDto);
    }
}
