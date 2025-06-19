using ToDoApp.Server.Contracts;
using ToDoApp.Server.Models;
using ToDoApp.Server.Models.Entity;

namespace ToDoApp.Server.Services;

public class TaskService(IBaseRepository<Models.Entity.Task> taskRepo, IBaseRepository<TaskGroup> taskGroupRepo) : ITaskService
{
    public async Task<ResponseModel> GetAllTaskAsync()
    {
        return new()
        {
            Data = await taskRepo.GetAllAsync(),
            IsSuccess = true
        };
    }

    public async Task<ResponseModel> GetTaskByIdAsync(int id)
    {
        ResponseModel response = new();
        var result = await taskRepo.GetByIdAsync(id).ConfigureAwait(false);
        response.Data = new
        {
            result?.TaskId,
            result?.Title,
            result?.Description,
            ToDoDate = result?.ToDoDate != null ? result?.ToDoDate?.ToString("yyyy-MM-dd") : null,
            result?.CreateDate,
            result?.CompleteDate,
            result?.IsStarred,
            result?.IsCompleted,
            result?.TaskGroupId
        };
        response.IsSuccess = result != null;
        if (!response.IsSuccess)
        {
            response.Message = "Task not found";
        }
        return response;
    }

    public async Task<ResponseModel> AddTaskAsync(AddTaskRequestModel model)
    {
        ResponseModel response = new();
        var task = new Models.Entity.Task
        {
            Title = model.Title,
            Description = model.Description,
            ToDoDate = model.ToDoDate,
            IsStarred = model.IsStarred,
            IsCompleted = false,
            TaskGroupId = model.TaskGroupId,
            CreateDate = DateTime.Now
        };

        await taskRepo.AddAsync(task);
        response.Message = "task added successfully!!";
        response.IsSuccess = true;
        return response;
    }

    public async Task<ResponseModel> UpdateTaskAsync(int id, UpdateTaskRequestModel model)
    {
        ResponseModel response = new();
        var task = await taskRepo.GetByIdAsync(id).ConfigureAwait(false);
        if (task == null)
        {
            response.IsSuccess = false;
            response.Message = "Task not found";
            return response;
        }

        task.Title = model.Title;
        task.Description = model.Description;
        task.ToDoDate = model.ToDoDate;
        if (task.IsCompleted)
            task.CompleteDate = DateTime.Now;
        else
            task.CompleteDate = null;

        await taskRepo.UpdateAsync(task);
        response.Message = "task updated successfully!!";
        response.IsSuccess = true;
        return response;
    }

    public async Task<ResponseModel> DeleteAsync(int id)
    {
        var task = await taskRepo.GetByIdAsync(id).ConfigureAwait(false);
        if (task == null)
        {
            return new ResponseModel
            {
                IsSuccess = false,
                Message = "Task not found"
            };
        }
        await taskRepo.DeleteAsync(id);
        return new()
        {
            IsSuccess = true,
            Message = "Task deleted successfully"
        };
    }

    public async Task<ResponseModel> ToggleStarTaskAsync(int taskId)
    {
        ResponseModel response = new();
        var task = await taskRepo.GetByIdAsync(taskId);
        if (task == null)
        {
            response.IsSuccess = false;
            response.Message = "Task not found";
            return response;
        }

        task.IsStarred = !task.IsStarred;
        await taskRepo.UpdateAsync(task);
        response.IsSuccess = true;
        response.Message = "Task starred status updated successfully";

        return response;

    }

    public async Task<ResponseModel> UpdateTaskCompletionStatusAsync(int taskId)
    {
        ResponseModel response = new();
        var task = await taskRepo.GetByIdAsync(taskId).ConfigureAwait(false);
        if (task == null)
        {
            response.IsSuccess = false;
            response.Message = "Task not found";
            return response;
        }
        task.IsCompleted = !task.IsCompleted;
        if (task.IsCompleted)
            task.CompleteDate = DateTime.Now;
        else
            task.CompleteDate = null;
        await taskRepo.UpdateAsync(task);
        response.IsSuccess = true;
        response.Message = "Task completion status updated successfully";
        return response;
    }

    public async Task<ResponseModel> MoveTaskToExistingGroupAsync(int taskId, int groupId)
    {
        ResponseModel response = new();

        var task = await taskRepo.GetByIdAsync(taskId).ConfigureAwait(false);
        if (task == null)
        {
            response.IsSuccess = false;
            response.Message = "Task not found";
            return response;
        }
        var group = await taskGroupRepo.GetByIdAsync(groupId).ConfigureAwait(false);
        if (group == null)
        {
            response.IsSuccess = false;
            response.Message = "Task group not found";
            return response;
        }
        task.TaskGroupId = groupId;
        await taskRepo.UpdateAsync(task);
        response.IsSuccess = true;
        response.Message = "Task moved to existing group successfully";
        return response;
    }

    public async Task<ResponseModel> MoveTaskToNewGroup(int taskId, AddGroupRequestModel model)
    {
        ResponseModel response = new();

        var task = await taskRepo.GetByIdAsync(taskId).ConfigureAwait(false);
        if (task == null)
        {
            response.IsSuccess = false;
            response.Message = "Task not found";
            return response;
        }

        var group = new TaskGroup { GroupName = model.GroupName, IsEnableShow = true, SortBy = "My order" };

        await taskGroupRepo.AddAsync(group);
        task.TaskGroupId = group.GroupId;
        await taskRepo.UpdateAsync(task);
        response.IsSuccess = true;
        response.Message = "Task moved to new group successfully";

        return response;
    }
}

