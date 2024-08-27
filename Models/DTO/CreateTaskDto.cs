using System.ComponentModel.DataAnnotations;

namespace Models.DTO
{
    public class CreateTaskDto
    {
        [Required]
        public string TaskName { get; set; }
    }

    public class UpdateTaskDto
    {
        [Required]
        public string TaskName { get; set; }
        public bool IsCompleted { get; set; }
    }
}
