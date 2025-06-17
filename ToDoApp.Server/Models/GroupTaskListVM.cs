using ToDoApp.Server.Models.Entity;

namespace ToDoApp.Server.Models;

public class GroupTaskListVM 
{
    public int GroupId { get; set; }
    public string? GroupName { get; set; }
    public string SortBy { get; set; }
    public bool isEnableShow { get; set; }
    public List<Entity.Task>? TaskList { get; set; }
    public List<Entity.Task>? CompletedTaskList { get; set; } 
}
