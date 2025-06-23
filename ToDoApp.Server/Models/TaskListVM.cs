using ToDoApp.Server.Models.Entity;

namespace ToDoApp.Server.Models
{
    public class TaskListVM :Entity.Task
    {
        public List<SubTask>? SubTasks { get; set; }
    }
}
