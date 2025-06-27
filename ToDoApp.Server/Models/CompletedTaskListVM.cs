namespace ToDoApp.Server.Models
{
    public class CompletedTaskListVM : Entity.Task
    {
        public int SubTaskId { get; set; }
    }
}
