using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDo.Models
{
    public class UserTask
    {
        [Key]
        public int TaskId { get; set; }

        [Required]
        public string TaskName { get; set; }

        public int UserId { get; set; }

        public DateTime CreatedAt { get; set; }


        public bool IsCompleted { get; set; }

    }
}
