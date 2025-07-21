namespace ToDoApp.Server.Models
{
    public class UndoTaskMovedCacheModel
    {
        public bool IsExistedGroup { get; set; }
        public int TaskId { get; set; }
        public int PreviousGroupId { get; set; }
        public int NewGroupId { get; set; }
    }
}
