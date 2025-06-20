using ToDoApp.Server.Models;
using ToDoApp.Server.Models.Entity;

namespace ToDoApp.Server.Contracts;

public interface ITaskGroupService
{
    /// <summary>
    /// Get all task Groups
    /// </summary>
    /// <returns>return response <see cref="ResponseModel"/></returns>
    Task<ResponseModel> GetTaskGroupsAsync();

    /// <summary>
    /// Get task group by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>return response <see cref="ResponseModel"/></returns>
    Task<ResponseModel> GetTaskGroupByIdAsync(int id);

    /// <summary>
    /// Add task group
    /// </summary>
    /// <param name="model"></param>
    /// <returns>return response <see cref="ResponseModel"/></returns>
    Task<ResponseModel> AddGroupAsync(AddGroupRequestModel model);

    /// <summary>
    /// Add or update a task group
    /// </summary>
    /// <param name="model"></param>
    /// <returns>return response <see cref="ResponseModel"/></returns>
    Task<ResponseModel> UpdateGroupAsync(int id, UpdateGroupRequestModel model);

    /// <summary>
    /// delete task group 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ResponseModel> DeleteGroupAsync(int id);

    /// <summary>
    /// Retrieves all task groups along with their associated tasks.
    /// </summary>
    /// <returns></returns>
    Task<ResponseModel> GetAllGroupWithTaskListAsync();

    /// <summary>
    /// delete all completed task of a spacific group
    /// </summary>
    /// <param name="groupId"></param>
    /// <returns></returns>
    Task<ResponseModel> DeleteCompletedTaskAsync(int groupId);

    /// <summary>
    /// Get all starred task async
    /// </summary>
    /// <returns></returns>
    Task<ResponseModel> GetStarredTaskAsync();

    /// <summary>
    /// Update the visibility status of a task group.
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="isVisible"></param>
    /// <returns></returns>
    Task<ResponseModel> UpdateVisibilityStatusAsync(int groupId, bool isVisible);

    /// <summary>
    /// Update the sort order of tasks within a specific task group.
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="sort"></param>
    /// <returns></returns>
    Task<ResponseModel> UpdateSortByAsync(int groupId, string sort);

}
