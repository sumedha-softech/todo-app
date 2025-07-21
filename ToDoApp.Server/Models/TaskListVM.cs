using ToDoApp.Server.Models.Entity;

namespace ToDoApp.Server.Models
{
    public class TaskListVM :Entity.Task
    {
        public int SubTaskId { get; set; }
        public int GroupId { get; set; }
        public List<SubTask>? SubTasks { get; set; }
    }
}
