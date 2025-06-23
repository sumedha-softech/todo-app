using ToDoApp.Server.Contracts;
using ToDoApp.Server.Models;
using ToDoApp.Server.Models.Entity;

namespace ToDoApp.Server.Services;

public class TaskService(IBaseRepository<Models.Entity.Task> taskRepo, IBaseRepository<TaskGroup> taskGroupRepo, IBaseRepository<SubTask> subTaskRepo) : ITaskService
{
    public async Task<ResponseModel> GetAllTaskAsync()
    {
        // Fetch all tasks from the repository
        var tasks = await taskRepo.GetAllAsync();
        if (tasks == null || !tasks.Any())
        {
            return new ResponseModel
            {
                IsSuccess = false,
                Message = "No tasks found"
            };
        }

        var subTasks = await subTaskRepo.GetAllAsync();
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
        var result = await taskRepo.GetByIdAsync(id).ConfigureAwait(false);
        response.Data = new
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

        // Delete all subtasks associated with the task
        var subTasks = await subTaskRepo.QueryAsync().Where(x => x.TaskId == id).ToList();
        if (subTasks != null && subTasks.Any())
        {
            var subTaskIdsForDelete = subTasks.Select(x => x.SubTaskId).ToList();

            try
            {
                await subTaskRepo.ExecuteSqlAsync($"DELETE FROM SubTask WHERE SubTaskId IN ({string.Join(",", subTaskIdsForDelete)})");
            }
            catch (Exception)
            {

                throw;
            }
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

        if (!task.IsCompleted)
        {
            var subTasks = await subTaskRepo.QueryAsync().Where(x => x.TaskId == taskId && !x.IsCompleted).ToList();
            if (subTasks != null && subTasks.Count > 0)
            {
                var subTaskIds = subTasks.Select(x => x.SubTaskId).ToList();
                var query = $"Update SubTask Set IsCompleted=1, CompleteDate=@CompleteDate where SubTaskId IN ({string.Join(",", subTaskIds)})";
                await subTaskRepo.ExecuteSqlAsync(query, new { CompleteDate = DateTime.Now });
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

