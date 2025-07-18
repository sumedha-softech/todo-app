namespace ToDoApp.Server.Models
{
    public class UndoSubTaskMovedCacheModel
    {
        public bool IsExistedGroup { get; set; }
        public int SubTaskId { get; set; }
        public int TaskId { get; set; }
        public int MovedGroupId { get; set; }
    }
}
