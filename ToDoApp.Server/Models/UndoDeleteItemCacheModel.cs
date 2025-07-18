namespace ToDoApp.Server.Models
{
    public class UndoDeleteItemCacheModel
    {
        public string ItemType { get; set; } // e.g., "Task", "SubTask", etc.
        public int GroupId { get; set; }
        public List<int> TasksId { get; set; }
        public List<int> SubTasksId { get; set; }
    }
}
