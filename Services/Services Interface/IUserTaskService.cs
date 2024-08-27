using ToDo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDo.Services
{
    public interface IUserTaskService
    {
        Task<IEnumerable<UserTask>> GetAllTasksAsync();
        Task<UserTask> AddTaskAsync(UserTask newTask);

        Task<UserTask> UpdateTaskAsync(int id, UserTask updatedTask);

        Task<bool> DeleteTaskAsync(int id);




    }
}
