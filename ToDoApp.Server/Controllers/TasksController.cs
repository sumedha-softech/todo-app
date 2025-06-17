using Microsoft.AspNetCore.Mvc;
using ToDoApp.Server.Contracts;
using ToDoApp.Server.Models.Entity;

namespace ToDoApp.Server.Controllers;

[Route("[controller]")]
[ApiController]
public class TasksController(ITaskService taskService) : ControllerBase
{
    #region [Get All Task]
    [HttpGet("")]
    public async Task<IActionResult> Get()
    {
        var response = await taskService.GetAllTaskAsync();
        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }

    #endregion [Get All Task]

    #region [Get Task By Id]

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var response = await taskService.GetTaskByIdAsync(id);
        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }

    #endregion [Get Task By Id]

    #region [Add task]
    [HttpPost("")]
    public async Task<IActionResult> Post([FromBody] Models.Entity.Task model)
    {
        var response = await taskService.AddOrUpdateTaskAsync(0,model);
        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }

    #endregion [Add task]

    #region [Update task]
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id,[FromBody] Models.Entity.Task model)
    {
        var response = await taskService.AddOrUpdateTaskAsync(id,model);
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
        var response = await taskService.DeleteAsync(id);
        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }
        return Ok(response);
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

    #region [Move Task To New List]
    [HttpPatch("{taskId}/move")]
    public async Task<IActionResult> MoveTaskToNewList(int taskId, [FromBody] TaskGroup group)
    {
        var response = await taskService.MoveTaskToNewList(taskId, group);
        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }
    #endregion [Move Task To New List]
}

