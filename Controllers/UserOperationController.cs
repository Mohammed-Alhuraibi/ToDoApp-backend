using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.DTO;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using ToDo.Services;

namespace ToDo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Ensure all endpoints are secured
    public class UserOperationsController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<UserOperationsController> _logger;

        public UserOperationsController(ITaskService taskService, ILogger<UserOperationsController> logger)
        {
            _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Get user ID from JWT
        private int GetUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }

        // Create Task
        [HttpPost("tasks")]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto createTaskDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserId();

            var createdTask = await _taskService.CreateTaskAsync(userId, createTaskDto);

            return CreatedAtAction(nameof(CreateTask), new { taskId = createdTask.TaskId }, createdTask);
        }

        // Update Task
        [HttpPut("tasks/{taskId}")]
        public async Task<IActionResult> UpdateTask(int taskId, [FromBody] UpdateTaskDto updateTaskDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserId();

            var updatedTask = await _taskService.UpdateTaskAsync(userId, taskId, updateTaskDto);

            if (updatedTask == null)
                return NotFound();

            return Ok(updatedTask);
        }

        // Remove Task
        [HttpDelete("tasks/{taskId}")]
        public async Task<IActionResult> RemoveTask(int taskId)
        {
            var userId = GetUserId();

            var success = await _taskService.RemoveTaskAsync(userId, taskId);

            if (!success)
                return NotFound();

            return NoContent();
        }

        // Get Tasks
        [HttpGet("tasks")]
        public async Task<IActionResult> GetUserTasks()
        {
            var userId = GetUserId();

            var tasks = await _taskService.GetUserTasksAsync(userId);

            return Ok(tasks);
        }
    }
}
