using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTO;
using ToDo.Models;

namespace ToDo.Services
{
    public interface ITaskService
    {
        Task<UserTask> CreateTaskAsync(int userId, CreateTaskDto createTaskDto);
        Task<UserTask> UpdateTaskAsync(int userId, int taskId, UpdateTaskDto updateTaskDto);
        Task<bool> RemoveTaskAsync(int userId, int taskId);
        Task<IEnumerable<UserTask>> GetUserTasksAsync(int userId);
    }
}
