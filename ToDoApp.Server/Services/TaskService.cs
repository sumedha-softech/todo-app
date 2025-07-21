using System.Text.RegularExpressions;
using ToDoApp.Server.Contracts;
using ToDoApp.Server.Helper;
using ToDoApp.Server.Models;
using ToDoApp.Server.Models.Entity;

namespace ToDoApp.Server.Services;

public class TaskService(IBaseRepository<Models.Entity.Task> taskRepo, IBaseRepository<TaskGroup> taskGroupRepo, IBaseRepository<SubTask> subTaskRepo, ICacheService cacheService) : ITaskService
{
    public async Task<ResponseModel> GetAllTaskAsync()
    {
        // Fetch all tasks from the repository
        var tasks = await taskRepo.QueryAsync().Where(x => !x.IsDeleted).ToList();
        if (tasks == null || !tasks.Any())
        {
            return new ResponseModel
            {
                IsSuccess = false,
                Message = "No tasks found"
            };
        }

        var subTasks = await subTaskRepo.QueryAsync().Where(x => !x.IsDeleted).ToList();
        // Map the tasks to a list of TaskListVM
        var data = tasks.Select(task => new TaskListVM
        {
            TaskId = task.TaskId,
            Title = task.Title,
            Description = task.Description,
            ToDoDate = task.ToDoDate,
            CreateDate = task.CreateDate,
            CompleteDate = task.CompleteDate,
            IsStarred = task.IsStarred,
            IsCompleted = task.IsCompleted,
            TaskGroupId = task.TaskGroupId,
            SubTasks = subTasks != null && subTasks.Count() > 0 ? subTasks.Where(SubTask => SubTask.TaskId == task.TaskId).ToList() : null
        }).ToList();

        return new()
        {
            Data = data,
            IsSuccess = true
        };
    }

    public async Task<ResponseModel> GetTaskByIdAsync(int id)
    {
        ResponseModel response = new();
        var result = await taskRepo.GetByIdAsync(id);
        if (result == null || result.IsDeleted)
        {
            response.IsSuccess = false;
            response.Message = "Task not found";
            return response;
        }

        return new()
        {
            Data = new
            {
                result.TaskId,
                result.Title,
                result.Description,
                ToDoDate = result.ToDoDate != null ? result.ToDoDate?.ToString("yyyy-MM-dd") : null,
                result.CreateDate,
                result.CompleteDate,
                result.IsStarred,
                result.IsCompleted,
                result.TaskGroupId,
                SubTasks = await subTaskRepo.GetAllAsync() is var subTasks && subTasks != null && subTasks.Count() > 0
                ? subTasks.Where(subTask => subTask.TaskId == result.TaskId).ToList()
                : null
            },
            IsSuccess = true
        };
    }

    public async Task<ResponseModel> AddTaskAsync(AddTaskRequestModel model)
    {
        ResponseModel response = new();
        await taskRepo.AddAsync(new Models.Entity.Task
        {
            Title = model.Title,
            Description = model.Description,
            ToDoDate = model.ToDoDate,
            IsStarred = model.IsStarred,
            IsCompleted = false,
            TaskGroupId = model.TaskGroupId,
            CreateDate = DateTime.Now
        });
        response.Message = "task added successfully!!";
        response.IsSuccess = true;
        return response;
    }

    public async Task<ResponseModel> UpdateTaskAsync(int id, UpdateTaskRequestModel model)
    {
        ResponseModel response = new();
        var task = await taskRepo.GetByIdAsync(id);
        if (task == null || task.IsDeleted)
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
        var cacheModel = new UndoDeleteItemCacheModel();
        var task = await taskRepo.GetByIdAsync(id);
        if (task == null || task.IsDeleted)
        {
            return new ResponseModel
            {
                IsSuccess = false,
                Message = "Task not found"
            };
        }

        cacheModel.TasksId = [id];

        // Delete all subtasks associated with the task
        var subTasks = await subTaskRepo.QueryAsync().Where(x => x.TaskId == id && !x.IsDeleted).ToList();
        if (subTasks != null && subTasks.Any())
        {
            var subTaskIdsForDelete = subTasks.Select(x => x.SubTaskId).ToList();
            await subTaskRepo.ExecuteSqlAsync($"Update SubTask Set IsDeleted=1 where SubTaskId IN ({string.Join(",", subTaskIdsForDelete)})");
            cacheModel.SubTasksId = subTaskIdsForDelete;
        }
        // Soft delete the task by setting IsDeleted to true
        task.IsDeleted = true;
        await taskRepo.UpdateAsync(task);
        cacheService.SetData(ConstantVariables.CacheKeyForUndoItems, cacheModel, DateTimeOffset.Now.AddSeconds(3));
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
        if (task == null || task.IsDeleted)
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
        var task = await taskRepo.GetByIdAsync(taskId);
        if (task == null || task.IsDeleted)
        {
            response.IsSuccess = false;
            response.Message = "Task not found";
            return response;
        }

        if (!task.IsCompleted)
        {
            var subTasks = await subTaskRepo.QueryAsync().Where(x => x.TaskId == taskId && !x.IsCompleted && !x.IsDeleted).ToList();
            if (subTasks != null && subTasks.Count > 0)
            {
                var subTaskIds = subTasks.Select(x => x.SubTaskId).ToList();
                await subTaskRepo.ExecuteSqlAsync($"Update SubTask Set IsCompleted=1, CompleteDate=@CompleteDate where SubTaskId IN ({string.Join(",", subTaskIds)})", new { CompleteDate = DateTime.Now });
            }
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
        UndoTaskMovedCacheModel cacheModel = new();
        ResponseModel response = new();

        var task = await taskRepo.GetByIdAsync(taskId);
        if (task == null || task.IsDeleted)
        {
            response.IsSuccess = false;
            response.Message = "Task not found";
            return response;
        }
        var group = await taskGroupRepo.GetByIdAsync(groupId);
        if (group == null || group.IsDeleted)
        {
            response.IsSuccess = false;
            response.Message = "Task group not found";
            return response;
        }

        cacheModel.TaskId = taskId;
        cacheModel.IsExistedGroup = true;
        cacheModel.PreviousGroupId = task.TaskGroupId;
        cacheService.SetData(ConstantVariables.CacheKeyForUndoTaskMoved, cacheModel, DateTimeOffset.Now.AddSeconds(3));

        task.TaskGroupId = groupId;
        await taskRepo.UpdateAsync(task);
        response.IsSuccess = true;
        response.Message = "Task moved to existing group successfully";
        return response;
    }

    public async Task<ResponseModel> MoveTaskToNewGroup(int taskId, AddGroupRequestModel model)
    {
        UndoTaskMovedCacheModel cacheModel = new();
        ResponseModel response = new();

        var task = await taskRepo.GetByIdAsync(taskId);
        if (task == null || task.IsDeleted)
        {
            response.IsSuccess = false;
            response.Message = "Task not found";
            return response;
        }

        var group = new TaskGroup { GroupName = model.GroupName, IsEnableShow = true, SortBy = "My order" };

        await taskGroupRepo.AddAsync(group);

        cacheModel.TaskId = taskId;
        cacheModel.IsExistedGroup = false;
        cacheModel.NewGroupId = group.GroupId;
        cacheModel.PreviousGroupId = task.TaskGroupId;
        cacheService.SetData(ConstantVariables.CacheKeyForUndoTaskMoved, cacheModel, DateTimeOffset.Now.AddSeconds(3));
        task.TaskGroupId = group.GroupId;
        await taskRepo.UpdateAsync(task);
        response.IsSuccess = true;
        response.Message = "Task moved to new group successfully";

        return response;
    }
}

