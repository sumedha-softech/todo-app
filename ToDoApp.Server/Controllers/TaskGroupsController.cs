using Microsoft.AspNetCore.Mvc;
using ToDoApp.Server.Contracts;
using ToDoApp.Server.Models.Entity;

namespace ToDoApp.Server.Controllers;

[ApiController]
[Route("[controller]")]
// ToDo: Add server side validation.
public class TaskGroupsController(ITaskGroupService taskListService, ITaskService taskService) : ControllerBase
{
    #region [Get Task Group List]
    [HttpGet("")]
    public async Task<IActionResult> Get()
    {
        var response = await taskListService.GetTaskGroupsAsync();
        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }
    #endregion [Get Task Group List]

    #region [Get Task Group By Id]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var response = await taskListService.GetTaskGroupByIdAsync(id);
        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    #endregion [Get Task Group By Id]

    #region [Add Task Group]
    [HttpPost("")]
    public async Task<IActionResult> Post([FromBody] TaskGroup model)
    {
        var response = await taskListService.AddOrUpdateTaskGroupAsync(0, model);
        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }

    #endregion [Add Task Group]

    #region [Edit Task Group]
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] TaskGroup model)
    {
        var response = await taskListService.AddOrUpdateTaskGroupAsync(id, model);
        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }

    #endregion [Edit Task Group]

    #region [Delete Task Group]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await taskListService.DeleteGroupAsync(id);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
    #endregion [Delete Task Group]

    #region [Get All Group With Their Task]
    [HttpGet("tasks")]
    public async Task<IActionResult> GetAllGroupsTaskListAsync()
    {
        var response = await taskService.GetAllGroupWithTaskListAsync();
        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }

    #endregion [Get All Group With Their Task ]

    #region [Delete Completed Task]

    [HttpDelete("{id}/complete")]
    public async Task<IActionResult> DeleteCompletedTask(int id)
    {
        var response = await taskService.DeleteCompletedTaskAsync(id);
        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }
    #endregion [Delete Completed Task]

    #region [Get All Starred Task]
    [HttpGet("tasks/star")]
    public async Task<IActionResult> GetAllStarredTask()
    {
        var response = await taskService.GetStarredTaskAsync();
        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }
    #endregion [Get All Starred Task]
}

