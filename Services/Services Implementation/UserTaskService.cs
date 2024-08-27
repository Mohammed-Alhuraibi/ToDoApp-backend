using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo.Models;
using ToDo.Repositories.Interfaces;

namespace ToDo.Services
{
    public class UserTaskService : IUserTaskService
    {
        private readonly IUserTaskRepository _userTaskRepository;

        public UserTaskService(IUserTaskRepository userTaskRepository)
        {
            _userTaskRepository = userTaskRepository;
        }

        public async Task<IEnumerable<UserTask>> GetAllTasksAsync()
        {
            return await _userTaskRepository.GetAllTasksAsync();
        }

        public async Task<UserTask> AddTaskAsync(UserTask newTask)
        {
            await _userTaskRepository.AddTaskAsync(newTask);
            await _userTaskRepository.SaveChangesAsync();
            return newTask;
        }

        public async Task<UserTask> UpdateTaskAsync(int id, UserTask newTask)
        {
            var existingTask = await _userTaskRepository.GetTaskByIdAsync(id);
            if (existingTask == null)
            {
                return null;
            }

            existingTask.TaskName = newTask.TaskName;
            existingTask.IsCompleted = newTask.IsCompleted;

            await _userTaskRepository.UpdateTaskAsync(existingTask);
            await _userTaskRepository.SaveChangesAsync();

            return existingTask;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _userTaskRepository.GetTaskByIdAsync(id);
            if (task == null)
            {
                return false;
            }

            await _userTaskRepository.DeleteTaskAsync(task);
            await _userTaskRepository.SaveChangesAsync();
            return true;
        }
    }
}
