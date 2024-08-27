using Data;
using Microsoft.EntityFrameworkCore;
using ToDo.Models;
using ToDo.Repositories.Interfaces;

namespace ToDo.Repositories
{
    public class UserTaskRepository : IUserTaskRepository
    {
        private readonly AppDbContext _context;

        public UserTaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserTask>> GetAllTasksAsync()
        {
            return await _context.UserTasks.ToListAsync();
        }

        public async Task<UserTask> GetTaskByIdAsync(int id)
        {
            return await _context.UserTasks.FindAsync(id);
        }

        public async Task AddTaskAsync(UserTask task)
        {
            await _context.UserTasks.AddAsync(task);
        }

        public async Task UpdateTaskAsync(UserTask task)
        {
             _context.UserTasks.Update(task);
        }

        public async Task DeleteTaskAsync(UserTask task)
        {
            _context.UserTasks.Remove(task);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<UserTask> GetTaskByIdAsync(int id, int userId)
        {
            return await _context.UserTasks
                .FirstOrDefaultAsync(task => task.TaskId == id && task.UserId == userId);
        }

        public async Task<IEnumerable<UserTask>> GetTasksByUserIdAsync(int userId)
        {
            return await _context.UserTasks
                .Where(task => task.UserId == userId)
                .ToListAsync();
        }
    }
}
