using ToDoApp.Server.Models.Entity;

namespace ToDoApp.Server.Models;

public class GroupTaskListVM 
{
    public int GroupId { get; set; }
    public string? GroupName { get; set; }
    public string SortBy { get; set; }
    public bool isEnableShow { get; set; }
    public List<TaskListVM>? TaskList { get; set; }
    public List<CompletedTaskListVM>? CompletedTaskList { get; set; } 
}
