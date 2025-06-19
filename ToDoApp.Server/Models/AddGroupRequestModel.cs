using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Server.Models
{
    public class AddGroupRequestModel
    {
        [Required(ErrorMessage = "GroupName is required")]
        public string? GroupName { get; set; }
    }
}
