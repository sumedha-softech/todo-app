using NPoco;

namespace ToDoApp.Server.Models.Entity;

[TableName("TaskList")]
[PrimaryKey("ListId", AutoIncrement = true)]
public class TaskGroup
{
    public int ListId { get; set; }
    public string? ListName { get; set; }
    public bool IsEnableShow { get; set; }
    public string SortBy { get; set; }
}

