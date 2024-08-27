using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo.Models;

namespace ToDo.Repositories.Interfaces
{
    public interface IUserTaskRepository
    {
        Task<IEnumerable<UserTask>> GetAllTasksAsync();
        Task<UserTask> GetTaskByIdAsync(int id, int userId);

        Task AddTaskAsync(UserTask task);
        Task UpdateTaskAsync(UserTask task);
        Task DeleteTaskAsync(UserTask task);
        Task SaveChangesAsync();
        Task<UserTask> GetTaskByIdAsync(int id);
        Task<IEnumerable<UserTask>> GetTasksByUserIdAsync(int userId);
    }
}
