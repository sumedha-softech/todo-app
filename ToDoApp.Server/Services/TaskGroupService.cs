using System.Diagnostics;
using System.Text.RegularExpressions;
using ToDoApp.Server.Contracts;
using ToDoApp.Server.Models;
using ToDoApp.Server.Models.Entity;

namespace ToDoApp.Server.Services;

public class TaskGroupService(IBaseRepository<TaskGroup> taskGroupRepo, IBaseRepository<Models.Entity.Task> taskRepo, IBaseRepository<SubTask> subTaskRepo) : ITaskGroupService
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

        var tasks = await taskRepo.QueryAsync().Where(x => x.TaskGroupId == id).ToList();

        if (tasks.Any())
        {
            var taskIds = tasks.Select(x => x.TaskId).ToList();
            // Delete all subtasks associated with the tasks in this group
            var subTasks = await subTaskRepo.QueryAsync().Where(x => taskIds.Contains(x.TaskId)).ToList();
            if (subTasks.Any())
            {
                var subTaskIds = string.Join(",", subTasks.Select(x => x.SubTaskId).ToList());
                await subTaskRepo.ExecuteSqlAsync($"DELETE FROM SubTask WHERE SubTaskId IN ({subTaskIds})");
            }

            // Delete all tasks associated with this group
            var taskIdsForDelete = string.Join(",", taskIds);
            await taskRepo.ExecuteSqlAsync($"DELETE FROM Task WHERE TaskId IN ({taskIdsForDelete})");
        }

        await taskGroupRepo.DeleteAsync(id);
        return new() { IsSuccess = true, Message = "Task Group deleted successfully" };
    }

    public async Task<ResponseModel> GetAllGroupWithTaskListAsync()
    {
        ResponseModel response = new();
        var taskList = await taskRepo.GetAllAsync();
        var subTaskList = await subTaskRepo.GetAllAsync();
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
                               x.TaskId.ToString())
                               .Select(x => new TaskListVM()
                               {
                                   TaskId = x.TaskId,
                                   Title = x.Title,
                                   Description = x.Description,
                                   ToDoDate = x.ToDoDate,
                                   CreateDate = x.CreateDate,
                                   CompleteDate = x.CompleteDate,
                                   IsStarred = x.IsStarred,
                                   IsCompleted = x.IsCompleted,
                                   TaskGroupId = x.TaskGroupId,
                                   SubTasks = subTaskList.Where(subTask => subTask.TaskId == x.TaskId && !subTask.IsCompleted).ToList()
                               }).ToList(),
            CompletedTaskList = completedTaskLists(group.GroupId, taskList.ToList(), subTaskList.ToList())
        }).ToList();

        response.IsSuccess = true;

        return response;
    }

    private List<CompletedTaskListVM> completedTaskLists(int groupId, List<Models.Entity.Task> tasks, List<SubTask> subTasks)
    {
        var groupedTask = tasks.Where(t => t.TaskGroupId == groupId);
        if (groupedTask == null || groupedTask.Count() == 0) return new();

        var completedTasks = groupedTask
        .Where(t => t.IsCompleted)
        .Select(t => new CompletedTaskListVM
        {
            TaskId = t.TaskId,
            SubTaskId = 0,
            TaskGroupId = t.TaskGroupId,
            Title = t.Title,
            Description = t.Description,
            ToDoDate = t.ToDoDate,
            CreateDate = t.CreateDate,
            CompleteDate = t.CompleteDate,
            IsStarred = t.IsStarred,
            IsCompleted = t.IsCompleted
        }).ToList();

        // Completed subtasks for those tasks
        if (subTasks != null && subTasks.Count > 0)
        {
            var completedSubTasks = subTasks
                .Where(st => st.IsCompleted && groupedTask.Any(ct => ct.TaskId == st.TaskId))
                .Select(st => new CompletedTaskListVM
                {
                    TaskId = st.TaskId,
                    SubTaskId = st.SubTaskId,
                    TaskGroupId = 0,
                    Title = st.Title,
                    Description = st.Description,
                    ToDoDate = st.ToDoDate,
                    CreateDate = st.CreateDate,
                    CompleteDate = st.CompleteDate,
                    IsStarred = st.IsStarred,
                    IsCompleted = st.IsCompleted
                }).ToList();

            // Combine both lists
            completedTasks.AddRange(completedSubTasks);
        }
        return completedTasks;
    }

    public async Task<ResponseModel> DeleteCompletedTaskAsync(int groupId)
    {
        var completedTasks = await taskRepo.QueryToGetRecordAsync("Select * from Task where TaskGroupId =@0 and IsCompleted =1", groupId).ConfigureAwait(false);
        if (completedTasks != null && completedTasks.Count() > 0)
        {
            var completedTaskIds = completedTasks.Select(x => x.TaskId).ToList();
            // Delete all subtasks associated with the completed tasks in this group
            var subTasks = await subTaskRepo.QueryAsync().Where(x => completedTaskIds.Contains(x.TaskId)).ToList();
            if (subTasks.Any())
            {
                var subTaskIds = string.Join(",", subTasks.Select(x => x.SubTaskId));
                await subTaskRepo.ExecuteSqlAsync($"DELETE FROM SubTask WHERE SubTaskId IN ({subTaskIds})");
            }

            // Delete all completed tasks associated with this group
            var taskIdsForDelete = string.Join(",", completedTaskIds);
            await taskRepo.ExecuteSqlAsync($"DELETE FROM Task WHERE TaskId IN ({taskIdsForDelete})");
        }

        return new()
        {
            IsSuccess = true,
            Message = "Completed tasks deleted successfully"
        };
    }

    public async Task<ResponseModel> GetStarredTaskAsync()
    {
        ResponseModel response = new();
        var group = await taskGroupRepo.GetByIdAsync(1);
        var subTaskList = await subTaskRepo.QueryAsync().ToList();
        response.Data = new GroupTaskListVM
        {
            GroupId = group.GroupId,
            GroupName = "Starred tasks",
            SortBy = group.SortBy ?? "My order",
            isEnableShow = true,

            TaskList = taskRepo.GetAllAsync().Result.Where(x => x.IsStarred && !x.IsCompleted)
                               .OrderBy(x => group.SortBy == "Title" ? x.Title :
                               group.SortBy == "Date" ? (x.ToDoDate ?? DateTime.MaxValue).ToString() :
                               group.SortBy == "Description" ? x.Description :
                               x.TaskId.ToString()).Select(x => new TaskListVM()
                               {
                                   TaskId = x.TaskId,
                                   Title = x.Title,
                                   Description = x.Description,
                                   ToDoDate = x.ToDoDate,
                                   CreateDate = x.CreateDate,
                                   CompleteDate = x.CompleteDate,
                                   IsStarred = x.IsStarred,
                                   IsCompleted = x.IsCompleted,
                                   TaskGroupId = x.TaskGroupId,
                                   SubTasks = subTaskList.Where(subTask => subTask.TaskId == x.TaskId && !subTask.IsCompleted).ToList()
                               }).ToList(),
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
