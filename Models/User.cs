using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ToDo.Models
{
    public class User
    {
        public User()
        {
            Tasks = new HashSet<UserTask>();
        }

        public int UserId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string UserName { get; set; }

        public DateTime CreatedAt { get; set; }
        public ICollection<UserTask> Tasks { get; set; }

        public string? ConfirmationCode { get; set; }
        public bool IsConfirmed { get; set; }

        public bool IsLoggedIn { get; set; }

        public string? ResetCode { get; set; } 



    }
}
