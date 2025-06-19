using NPoco;

namespace ToDoApp.Server.Models.Entity;

[TableName("Group")]
[PrimaryKey("GroupId", AutoIncrement = true)]
public class TaskGroup
{
    public int GroupId { get; set; }
    public string? GroupName { get; set; }
    public bool? IsEnableShow { get; set; }
    public string? SortBy { get; set; }
}

