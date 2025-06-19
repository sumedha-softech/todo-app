using ToDoApp.Server.Contracts;
using ToDoApp.Server.Models;
using ToDoApp.Server.Models.Entity;

namespace ToDoApp.Server.Services;

public class TaskGroupService(IBaseRepository<TaskGroup> taskGroupRepo, IBaseRepository<Models.Entity.Task> taskRepo) : ITaskGroupService
{
    public async Task<ResponseModel> GetTaskGroupsAsync()
    {
        return new()
        {
            Data = await taskGroupRepo.GetAllAsync().ConfigureAwait(false),
            IsSuccess = true
        };
    }

    public async Task<ResponseModel> GetTaskGroupByIdAsync(int id)
    {
        ResponseModel response = new();
        try
        {
            response.Data = await taskGroupRepo.GetByIdAsync(id).ConfigureAwait(false) ?? new();
            response.IsSuccess = true;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ResponseModel> AddGroupAsync(AddGroupRequestModel model)
    {
        await taskGroupRepo.AddAsync(new TaskGroup { GroupName = model.GroupName, IsEnableShow = true, SortBy = "My order" });
        return new()
        {
            IsSuccess = true,
            Message = "Task Group added successfully"
        };
    }

    public async Task<ResponseModel> UpdateGroupAsync(int id, UpdateGroupRequestModel model)
    {
        var group = await taskGroupRepo.GetByIdAsync(id).ConfigureAwait(false);

        if (group == null)
        {
            return new ResponseModel
            {
                IsSuccess = false,
                Message = "Task Group not found"
            };
        }

        group.GroupName = model.GroupName;
        await taskGroupRepo.UpdateAsync(group);
        return new ResponseModel
        {
            IsSuccess = true,
            Message = "Task Group updated successfully"
        }; ;
    }

    public async Task<ResponseModel> DeleteGroupAsync(int id)
    {
        var group = await taskGroupRepo.GetByIdAsync(id).ConfigureAwait(false);
        if (group == null)
        {
            return new ResponseModel
            {
                IsSuccess = false,
                Message = "Task Group not found"
            };
        }
        var tasks = await taskRepo.QueryNewAsync().Where(x => x.TaskGroupId == id).ToList();
        foreach (var item in tasks)
        {
            await taskRepo.DeleteAsync(item.TaskId);
        }
        await taskGroupRepo.DeleteAsync(id);
        return new() { IsSuccess = true, Message = "Task Group deleted successfully" };
    }

    public async Task<ResponseModel> GetAllGroupWithTaskListAsync()
    {
        ResponseModel response = new();
        var taskList = await taskRepo.GetAllAsync().ConfigureAwait(false);
        response.Data = taskGroupRepo.GetAllAsync().Result.Select(group => new GroupTaskListVM
        {
            GroupId = group.GroupId,
            GroupName = group.GroupName,
            SortBy = group.SortBy ?? "My order",
            isEnableShow = group.IsEnableShow ?? true,
            TaskList = taskList.Where(x => x.TaskGroupId == group.GroupId && !x.IsCompleted)
                               .OrderBy(x => group.SortBy == "Title" ? x.Title :
                               group.SortBy == "Date" ? (x.ToDoDate ?? DateTime.MaxValue).ToString() :
                               group.SortBy == "Description" ? x.Description :
                               x.TaskId.ToString()).ToList(),
            CompletedTaskList = taskList.Where(x => x.TaskGroupId == group.GroupId && x.IsCompleted).ToList()
        }).ToList();
        response.IsSuccess = true;

        return response;
    }

    public async Task<ResponseModel> DeleteCompletedTaskAsync(int groupId)
    {
        ResponseModel response = new();
        var completedTasks = await taskRepo.QueryAsync("Select * from Task where TaskGroupId =@0 and IsCompleted =1", groupId).ConfigureAwait(false);
        foreach (var task in completedTasks)
        {
            await taskRepo.DeleteAsync(task.TaskId);
        }
        response.IsSuccess = true;
        response.Message = "Completed tasks deleted successfully";

        return response;
    }

    public async Task<ResponseModel> GetStarredTaskAsync()
    {
        ResponseModel response = new();
        var group = await taskGroupRepo.GetByIdAsync(1);
        response.Data = new GroupTaskListVM
        {
            GroupId = group.GroupId,
            GroupName = "Starred tasks",
            SortBy = group.SortBy,
            isEnableShow = true,
            TaskList = taskRepo.GetAllAsync().Result.Where(x => x.IsStarred && !x.IsCompleted)
                               .OrderBy(x => group.SortBy == "Title" ? x.Title :
                               group.SortBy == "Date" ? x.ToDoDate.ToString() :
                               group.SortBy == "Description" ? x.Description :
                               x.TaskId.ToString()).ToList(),
            CompletedTaskList = []
        };
        response.IsSuccess = true;
        return response;
    }

    public async Task<ResponseModel> UpdateVisibilityStatusAsync(int groupId, bool isVisible)
    {
        var group = await taskGroupRepo.GetByIdAsync(groupId);
        if (group == null)
        {
            return new ResponseModel
            {
                IsSuccess = false,
                Message = "Task Group not found"
            };
        }
        group.IsEnableShow = isVisible;
        await taskGroupRepo.UpdateAsync(group);
        return new ResponseModel
        {
            IsSuccess = true,
            Message = isVisible ? "Task Group is now visible" : "Task Group is now hidden"
        };
    }

    public async Task<ResponseModel> UpdateSortByAsync(int groupId, string sort)
    {
        var group = await taskGroupRepo.GetByIdAsync(groupId);
        if (group == null)
        {
            return new ResponseModel
            {
                IsSuccess = false,
                Message = "Task Group not found"
            };
        }

        group.SortBy = sort;
        await taskGroupRepo.UpdateAsync(group);
        return new ResponseModel
        {
            IsSuccess = true,
            Message = "Sort order updated successfully"
        };
    }
}
