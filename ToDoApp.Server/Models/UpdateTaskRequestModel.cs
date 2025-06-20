using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Server.Models
{
    public class UpdateTaskRequestModel
    {
        [Required(ErrorMessage = "Title is required!")]
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? ToDoDate { get; set; }
    }
}
