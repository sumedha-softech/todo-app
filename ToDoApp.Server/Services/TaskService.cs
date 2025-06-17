using ToDoApp.Server.Contracts;
using ToDoApp.Server.Models;
using ToDoApp.Server.Models.Entity;

namespace ToDoApp.Server.Services;

public class TaskService(IBaseRepository<Models.Entity.Task> taskRepo, IBaseRepository<TaskGroup> taskGroupRepo) : ITaskService
{
    public async Task<ResponseModel> GetAllTaskAsync()
    {
        ResponseModel response = new();

        try
        {
            response.Data = await taskRepo.GetAllAsync();
            response.IsSuccess = true;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ResponseModel> GetTaskByIdAsync(int id)
    {
        ResponseModel response = new();
        try
        {
            var result = await taskRepo.GetByIdAsync(id);
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


            response.IsSuccess = true;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ResponseModel> AddOrUpdateTaskAsync(int id, Models.Entity.Task task)
    {
        ResponseModel response = new();
        try
        {
            // ToDo: fix this.
            if (id == 0)
            {
                task.TaskId = 0;
                task.CompleteDate = null;
                task.CreateDate = DateTime.Now;
                await taskRepo.AddAsync(task);
                response.Message = "task added successfully!!";
            }
            else
            {
                task.TaskId = id;
                if (task.IsCompleted)
                    task.CompleteDate = DateTime.Now;
                else
                    task.CompleteDate = null;
                await taskRepo.UpdateAsync(task);
                response.Message = "task updated successfully!!";
            }
            response.IsSuccess = true;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ResponseModel> DeleteAsync(int id)
    {
        ResponseModel response = new();
        try
        {
            await taskRepo.DeleteAsync(id);
            response.IsSuccess = true;
            response.Message = "Task deleted successfully";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ResponseModel> GetAllGroupWithTaskListAsync()
    {
        ResponseModel response = new();
        try
        {
            // ToDo: fix this.
            var groupList = await taskGroupRepo.GetAllAsync();
            var taskList = await taskRepo.GetAllAsync();
            response.Data = groupList.Select(group => new GroupTaskListVM
            {
                GroupId = group.ListId,
                GroupName = group.ListName,
                SortBy = group.SortBy,
                isEnableShow = group.IsEnableShow,
                TaskList = taskList.Where(x => x.TaskGroupId == group.ListId && !x.IsCompleted)
                                   .OrderBy(x => group.SortBy == "Title" ? x.Title :
                                   group.SortBy == "Date" ? x.ToDoDate.ToString() :
                                   group.SortBy == "Description" ? x.Description :
                                   x.TaskId.ToString()).ToList(),
                CompletedTaskList = taskList.Where(x => x.TaskGroupId == group.ListId && x.IsCompleted).ToList()
            }).ToList();
            response.IsSuccess = true;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }

        return response;
    }

    public async Task<ResponseModel> ToggleStarTaskAsync(int taskId)
    {
        ResponseModel response = new();
        try
        {
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
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;

    }

    public async Task<ResponseModel> GetStarredTaskAsync()
    {
        ResponseModel response = new();
        try
        {
            // ToDo: fix this.
            var group = await taskGroupRepo.GetByIdAsync(1).ConfigureAwait(false);
            var starredTasks = await taskRepo.GetAllAsync().ConfigureAwait(false);
            response.Data = new GroupTaskListVM
            {
                GroupId = group.ListId,
                GroupName = "Starred tasks",
                SortBy = group.SortBy,
                isEnableShow = true,
                TaskList = starredTasks.Where(x => x.IsStarred && !x.IsCompleted)
                                   .OrderBy(x => group.SortBy == "Title" ? x.Title :
                                   group.SortBy == "Date" ? x.ToDoDate.ToString() :
                                   group.SortBy == "Description" ? x.Description :
                                   x.TaskId.ToString()).ToList(),
                CompletedTaskList = []
            };
            response.IsSuccess = true;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ResponseModel> DeleteCompletedTaskAsync(int groupId)
    {
        ResponseModel response = new();
        try
        {
            // ToDo: @0 why parameter name is 0 or 1
            var completedTasks = await taskRepo.QueryAsync("Select * from Task where TaskGroupId =@0 and IsCompleted =1", groupId).ConfigureAwait(false);
            foreach (var task in completedTasks)
            {
                try
                {
                    await taskRepo.DeleteAsync(task.TaskId);
                }
                catch (Exception)
                {
                    // ToDo handle error
                }
            }
            response.IsSuccess = true;
            response.Message = "Completed tasks deleted successfully";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ResponseModel> MoveTaskToNewList(int taskId, TaskGroup group)
    {
        ResponseModel response = new();
        try
        {
            var task = await taskRepo.GetByIdAsync(taskId).ConfigureAwait(false);
            if (task == null)
            {
                response.IsSuccess = false;
                response.Message = "Task not found";
                return response;
            }

            await taskGroupRepo.AddAsync(group);
            // ToDo: handle error
            if(group.ListId == 0) return response;

            task.TaskGroupId = group.ListId;
            await taskRepo.UpdateAsync(task);
            response.IsSuccess = true;
            response.Message = "Task moved to new group successfully";

        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }
}

