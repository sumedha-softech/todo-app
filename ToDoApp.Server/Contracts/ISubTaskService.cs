using ToDoApp.Server.Models;

namespace ToDoApp.Server.Contracts
{
    public interface ISubTaskService
    {

        /// <summary>
        /// Get all Subtask 
        /// </summary>
        /// <returns>return response <see cref="ResponseModel"/></returns>
        Task<ResponseModel> GetAllSubTaskAsync();

        /// <summary>
        /// Get sub task by id
        /// </summary>
        /// <param name="subTaskId"></param>
        /// <returns></returns>
        Task<ResponseModel> GetSubTaskByIdAsync(int subTaskId);

        /// <summary>
        /// Add a Sub task
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseModel> AddSubTaskAsync(AddSubTaskRequestModel model);

        /// <summary>
        /// update subtask
        /// </summary>
        /// <param name="subTaskId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseModel> UpdateSubTaskAsync(int subTaskId, UpdateTaskRequestModel model);

        /// <summary>
        /// delete a subtask
        /// </summary>
        /// <param name="subTaskId"></param>
        /// <returns></returns>
        Task<ResponseModel> DeleteAsync(int subTaskIdd);

        /// <summary>
        /// update subtask for starred task
        /// </summary>
        /// <param name="subTaskId"></param> 
        /// <returns></returns>
        Task<ResponseModel> ToggleStarSubTaskAsync(int subTaskId);

        /// <summary>
        /// toggle subtask completion status
        /// </summary>
        /// <param name="subTaskId"></param>
        /// <returns></returns>
        Task<ResponseModel> UpdateSubTaskCompletionStatusAsync(int subTaskId);

        /// <summary>
        /// move subtask to a existing group
        /// </summary>
        /// <param name="subTaskId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<ResponseModel> MoveSubTaskToExistingGroupAsync(int taskId, int subTaskId);

        /// <summary>
        /// move subtask to a new group
        /// </summary>
        /// <param name="subTaskId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseModel> MoveSubTaskToNewGroup(int subTaskId, AddGroupRequestModel model);

    }
}
