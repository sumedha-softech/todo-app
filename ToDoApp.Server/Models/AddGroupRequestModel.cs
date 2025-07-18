using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Server.Models
{
    public class AddGroupRequestModel
    {
        [Required(ErrorMessage = "GroupName is required")]
        [StringLength(100, ErrorMessage = "Group name must be at most 100 characters long.")]
        public string? GroupName { get; set; }
    }
}
