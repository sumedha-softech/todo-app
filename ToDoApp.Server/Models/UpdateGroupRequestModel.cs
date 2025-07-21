using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Server.Models
{
    public class UpdateGroupRequestModel
    {
        [Required(ErrorMessage ="Group name is required!")]
        [StringLength(100, ErrorMessage = "Group name must be at most 100 characters long.")]
        public string? GroupName { get; set; }
    }
}
