using ToDoApp.Server.Models;

namespace ToDoApp.Server.Contracts;

public interface ITaskService
{
    /// <summary>
    /// Get all task 
    /// </summary>
    /// <returns>return response <see cref="ResponseModel"/></returns>
    Task<ResponseModel> GetAllTaskAsync();

    /// <summary>
    /// Get task by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ResponseModel> GetTaskByIdAsync(int id);

    /// <summary>
    /// Add a task
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<ResponseModel> AddTaskAsync(AddTaskRequestModel model);

    /// <summary>
    /// update task
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<ResponseModel> UpdateTaskAsync(int id, UpdateTaskRequestModel model);

    /// <summary>
    /// delete a task
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>
    Task<ResponseModel> DeleteAsync(int id);

    /// <summary>
    /// update task for starred task
    /// </summary>
    /// <returns></returns>
    Task<ResponseModel> ToggleStarTaskAsync(int taskId);

    /// <summary>
    /// toggle task completion status
    /// </summary>
    /// <returns></returns>
    Task<ResponseModel> UpdateTaskCompletionStatusAsync(int taskId);

    /// <summary>
    /// move task to a existing group
    /// </summary>
    /// <returns></returns>
    Task<ResponseModel> MoveTaskToExistingGroupAsync(int taskId, int groupId);

    /// <summary>
    /// move task to a new group
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="group"></param>
    /// <returns></returns>
    Task<ResponseModel> MoveTaskToNewGroup(int taskId, AddGroupRequestModel model);

}
