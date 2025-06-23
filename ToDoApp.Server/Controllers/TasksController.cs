using Microsoft.AspNetCore.Mvc;
using ToDoApp.Server.Contracts;
using ToDoApp.Server.Models;

namespace ToDoApp.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TasksController(ITaskService taskService) : ControllerBase
{
    #region [Get All Task]
    [HttpGet("")]
    public async Task<IActionResult> Get() => Ok(await taskService.GetAllTaskAsync());

    #endregion [Get All Task]

    #region [Get Task By Id]

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id) => Ok(await taskService.GetTaskByIdAsync(id));

    #endregion [Get Task By Id]

    #region [Add task]
    [HttpPost("")]
    public async Task<IActionResult> Post([FromBody] AddTaskRequestModel model) => Ok(await taskService.AddTaskAsync(model));
    #endregion [Add task]

    #region [Update task]
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] UpdateTaskRequestModel model)
    {
        var response = await taskService.UpdateTaskAsync(id, model);
        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }
    #endregion [Update task]

    #region [Delete Task]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await taskService.DeleteAsync(id);
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }
    #endregion [Delete Task]

    #region [Toggle Star Task]
    [HttpPatch("{taskId}/star")]
    public async Task<IActionResult> ToggleStarTask(int taskId)
    {
        var response = await taskService.ToggleStarTaskAsync(taskId);
        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }
    #endregion [Toggle Star Task]

    #region [Move Task To New Group]
    [HttpPatch("{taskId}/move")]
    public async Task<IActionResult> MoveTaskToNewGroup(int taskId, [FromBody] AddGroupRequestModel model)
    {
        var response = await taskService.MoveTaskToNewGroup(taskId, model);
        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }
    #endregion [Move Task To New List]

    #region [Move Task To Existing Group]
    [HttpPatch("{taskId}/move/{groupId}")]
    public async Task<IActionResult> MoveTaskToExistingGroup(int taskId, int groupId)
    {
        var response = await taskService.MoveTaskToExistingGroupAsync(taskId, groupId);
        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }
    #endregion [Move Task To Existing Group]

    #region [Toggle Complete Task]
    [HttpPatch("{taskId}/complete")]
    public async Task<IActionResult> UpdateTaskCompletionStatus(int taskId)
    {
        var response = await taskService.UpdateTaskCompletionStatusAsync(taskId);
        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }
    #endregion [Toggle Complete Task]

}

