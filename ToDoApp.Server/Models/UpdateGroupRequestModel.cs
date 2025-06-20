using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Server.Models
{
    public class UpdateGroupRequestModel
    {
        [Required(ErrorMessage ="Group name is required!")]
        public string? GroupName { get; set; }
    }
}
