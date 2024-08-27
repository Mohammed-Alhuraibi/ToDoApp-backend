using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo.Models;
using ToDo.Repositories.Interfaces;

namespace ToDo.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<User> AddUserAsync(User newUser)
        {
            await _userRepository.AddUserAsync(newUser);
            await _userRepository.SaveChangesAsync();
            return newUser;
        }

        public async Task<User> UpdateUserAsync(int id, User updatedUser)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return null;
            }

            existingUser.Email = updatedUser.Email;
            existingUser.Password = updatedUser.Password;
            existingUser.UserName = updatedUser.UserName;

            await _userRepository.UpdateUserAsync(existingUser);
            await _userRepository.SaveChangesAsync();

            return existingUser;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return false;
            }

            await _userRepository.DeleteUserAsync(user);
            await _userRepository.SaveChangesAsync();
            return true;
        }
    }
}
