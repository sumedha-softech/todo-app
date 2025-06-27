using Microsoft.AspNetCore.Mvc;
using ToDoApp.Server.Contracts;
using ToDoApp.Server.Models;
using ToDoApp.Server.Services;

namespace ToDoApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubTasksController(ISubTaskService subTasksService) : ControllerBase
    {
        #region [Get All Sub Task]
        [HttpGet("")]
        public async Task<IActionResult> Get() => Ok(await subTasksService.GetAllSubTaskAsync());

        #endregion [Get All Sub Task]

        #region [Get Sub Task By Id]

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await subTasksService.GetSubTaskByIdAsync(id);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        #endregion [Get Sub Task By Id]

        #region [Add Sub task]
        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody] AddSubTaskRequestModel model) => Ok(await subTasksService.AddSubTaskAsync(model));
        #endregion [Add Sub task]

        #region [Update Sub task]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateTaskRequestModel model)
        {
            var response = await subTasksService.UpdateSubTaskAsync(id, model);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        #endregion [Update Sub task]

        #region [Delete Sub Task]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await subTasksService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        #endregion [Delete Sub Task]

        #region [Toggle Star Sub Task]
        [HttpPatch("{taskId}/star")]
        public async Task<IActionResult> ToggleStarTask(int taskId)
        {
            var response = await subTasksService.ToggleStarSubTaskAsync(taskId);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        #endregion [Toggle Star Sub Task]

        #region [Move Sub Task To New Group]
        [HttpPatch("{subTaskId}/move")]
        public async Task<IActionResult> MoveTaskToNewGroup(int subTaskId, [FromBody] AddGroupRequestModel model)
        {
            var response = await subTasksService.MoveSubTaskToNewGroup(subTaskId, model);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        #endregion [Move Sub Task To New List]

        #region [Move Sub Task To Existing Group]
        [HttpPatch("{subTaskId}/move/{groupId}")]
        public async Task<IActionResult> MoveTaskToExistingGroup(int subTaskId, int groupId)
        {
            var response = await subTasksService.MoveSubTaskToExistingGroupAsync(subTaskId, groupId);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        #endregion [Move Sub Task To Existing Group]

        #region [Toggle Complete Sub Task]
        [HttpPatch("{subTaskId}/complete")]
        public async Task<IActionResult> UpdateTaskCompletionStatus(int subTaskId)
        {
            var response = await subTasksService.UpdateSubTaskCompletionStatusAsync(subTaskId);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        #endregion [Toggle Complete Task]
    }
}
