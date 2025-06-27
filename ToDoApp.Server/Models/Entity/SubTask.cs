using NPoco;

namespace ToDoApp.Server.Models.Entity
{
    [TableName("SubTask")]
    [PrimaryKey("SubTaskId", AutoIncrement = true)]
    public class SubTask
    {
        public int SubTaskId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? ToDoDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public bool IsStarred { get; set; }
        public bool IsCompleted { get; set; }
        public int TaskId { get; set; }
    }
}
