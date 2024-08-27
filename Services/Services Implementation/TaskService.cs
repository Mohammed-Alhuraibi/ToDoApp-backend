using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo.Models;
using Models.DTO;
using ToDo.Repositories.Interfaces;

namespace ToDo.Services
{
    public class TaskService : ITaskService
    {
        private readonly IUserTaskRepository _taskRepository;

        public TaskService(IUserTaskRepository taskRepository)
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
        }

        public async Task<UserTask> CreateTaskAsync(int userId, CreateTaskDto createTaskDto)
        {
            var userTask = new UserTask
            {
                TaskName = createTaskDto.TaskName,
                UserId = userId,
                IsCompleted = false
            };

            await _taskRepository.AddTaskAsync(userTask);
            await _taskRepository.SaveChangesAsync();
            return userTask;
        }

        public async Task<UserTask> UpdateTaskAsync(int userId, int taskId, UpdateTaskDto updateTaskDto)
        {
            var userTask = await _taskRepository.GetTaskByIdAsync(taskId, userId);

            if (userTask == null)
                return null;

            userTask.TaskName = updateTaskDto.TaskName;
            userTask.IsCompleted = updateTaskDto.IsCompleted;

            await _taskRepository.UpdateTaskAsync(userTask);
            await _taskRepository.SaveChangesAsync();
            return userTask;
        }

        public async Task<bool> RemoveTaskAsync(int userId, int taskId)
        {
            var userTask = await _taskRepository.GetTaskByIdAsync(taskId, userId);

            if (userTask == null)
                return false;

            await _taskRepository.DeleteTaskAsync(userTask);
            await _taskRepository.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<UserTask>> GetUserTasksAsync(int userId)
        {
            return await _taskRepository.GetTasksByUserIdAsync(userId);
        }
    }
}
