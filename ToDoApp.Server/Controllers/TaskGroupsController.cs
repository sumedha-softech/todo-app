using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Server.Contracts;
using ToDoApp.Server.Models;

namespace ToDoApp.Server.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TaskGroupsController(ITaskGroupService taskGroupService) : ControllerBase
{
    #region [Get Task Group List]
    [HttpGet("")]
    public async Task<IActionResult> Get() => Ok(await taskGroupService.GetTaskGroupsAsync());
    #endregion [Get Task Group List]

    #region [Get Task Group By Id]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id) => Ok(await taskGroupService.GetTaskGroupByIdAsync(id));

    #endregion [Get Task Group By Id]

    #region [Add Task Group]
    [HttpPost("")]
    public async Task<IActionResult> Post([FromBody] AddGroupRequestModel model) => Ok(await taskGroupService.AddGroupAsync(model));

    #endregion [Add Task Group]

    #region [Edit Task Group]
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] UpdateGroupRequestModel model)
    {
        var response = await taskGroupService.UpdateGroupAsync(id, model);
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
       var result= await taskGroupService.DeleteGroupAsync(id);
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }
    #endregion [Delete Task Group]

    #region [Get All Group With Their Task]
    [HttpGet("tasks")]
    public async Task<IActionResult> GetAllGroupsTaskListAsync() => Ok(await taskGroupService.GetAllGroupWithTaskListAsync());

    #endregion [Get All Group With Their Task ]

    #region [Delete Completed Task]
    [HttpDelete("{id}/complete")]
    public async Task<IActionResult> DeleteCompletedTask(int id) => Ok(await taskGroupService.DeleteCompletedTaskAsync(id));
    #endregion [Delete Completed Task]

    #region [Get All Starred Task]
    [HttpGet("tasks/star")]
    public async Task<IActionResult> GetAllStarredTask()=> Ok(await taskGroupService.GetStarredTaskAsync());
    #endregion [Get All Starred Task]

    #region [Update Task Group Visibility]
    [HttpPatch("{groupId}/visibility/{isVisible}")]
    public async Task<IActionResult> UpdateGroupVisibility(int groupId, bool isVisible)
    {
        var response = await taskGroupService.UpdateVisibilityStatusAsync(groupId, isVisible);
        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }
    #endregion [Update Task Group Visibility]

    #region [Update SortBy]
    [HttpPatch("{groupId}/sortBy/{sort}")]
    public async Task<IActionResult> UpdateSortBy(int groupId, string sort)
    {
        var response = await taskGroupService.UpdateSortByAsync(groupId, sort);
        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }
    #endregion [Update SortBy]
}

