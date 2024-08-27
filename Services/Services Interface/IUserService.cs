using ToDo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDo.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> AddUserAsync(User newUser);
        Task<User> UpdateUserAsync(int id, User updatedUser);
        Task<bool> DeleteUserAsync(int id);
    }
}
