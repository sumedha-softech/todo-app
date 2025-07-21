using ToDoApp.Server.Contracts;
using ToDoApp.Server.Helper;
using ToDoApp.Server.Models;
using ToDoApp.Server.Models.Entity;

namespace ToDoApp.Server.Services;

public class TaskGroupService(IBaseRepository<TaskGroup> taskGroupRepo, IBaseRepository<Models.Entity.Task> taskRepo, IBaseRepository<SubTask> subTaskRepo, ICacheService cacheService) : ITaskGroupService
{
    public async Task<ResponseModel> GetTaskGroupsAsync()
    {
        return new()
        {
            Data = await taskGroupRepo.QueryAsync().Where(x => !x.IsDeleted).ToList(),
            IsSuccess = true
        };
    }

    public async Task<ResponseModel> GetTaskGroupByIdAsync(int id)
    {
        var taskGroup = await taskGroupRepo.GetByIdAsync(id);
        if (taskGroup == null || taskGroup.IsDeleted)
        {
            return new ResponseModel
            {
                IsSuccess = false,
                Message = "Task Group not found"
            };
        }

        return new()
        {
            Data = taskGroup,
            IsSuccess = true

        };
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

        if (group == null || group.IsDeleted)
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
        };
    }

    public async Task<ResponseModel> DeleteGroupAsync(int id)
    {
        var cacheModel = new UndoDeleteItemCacheModel();

        var group = await taskGroupRepo.GetByIdAsync(id);
        if (group == null || group.IsDeleted)
        {
            return new ResponseModel
            {
                IsSuccess = false,
                Message = "Task Group not found"
            };
        }

        cacheModel.ItemType = "Group";
        cacheModel.GroupId = id;

        var tasks = await taskRepo.QueryAsync().Where(x => x.TaskGroupId == id && !x.IsDeleted).ToList();

        if (tasks.Any())
        {
            var taskIds = tasks.Select(x => x.TaskId).ToList();

            // Delete all subtasks associated with the tasks in this group
            var subTasks = await subTaskRepo.QueryAsync().Where(x => taskIds.Contains(x.TaskId) && !x.IsDeleted).ToList();
            if (subTasks.Any())
            {
                var subTaskIds = subTasks.Select(x => x.SubTaskId).ToList();
                await subTaskRepo.ExecuteSqlAsync($"UPDATE SubTask SET IsDeleted = 1 WHERE SubTaskId IN ({string.Join(",", subTaskIds)})");
                cacheModel.SubTasksId = subTaskIds;
            }

            // Delete all tasks associated with this group
            var taskIdsForDelete = string.Join(",", taskIds);
            await taskRepo.ExecuteSqlAsync($"UPDATE Task SET IsDeleted = 1 WHERE TaskId IN ({taskIdsForDelete})");
            cacheModel.TasksId = taskIds;
        }

        group.IsDeleted = true;
        await taskGroupRepo.UpdateAsync(group);
        cacheService.SetData(ConstantVariables.CacheKeyForUndoItems, cacheModel, DateTimeOffset.Now.AddSeconds(3));
        return new() { IsSuccess = true, Message = "Task Group deleted successfully" };
    }

    public async Task<ResponseModel> GetAllGroupWithTaskListAsync()
    {
        ResponseModel response = new();
        var taskList = await taskRepo.QueryAsync().Where(x => !x.IsDeleted).ToList();
        var subTaskList = await subTaskRepo.QueryAsync().Where(x => !x.IsDeleted).ToList();
        response.Data = taskGroupRepo.QueryAsync().Where(x => !x.IsDeleted).ToList().Result.Select(group => new GroupTaskListVM
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
        var groupTaskList = await taskRepo.QueryAsync()?.Where(x => x.TaskGroupId == groupId && !x.IsDeleted)?.ToList();
        if (groupTaskList == null || groupTaskList.Count() == 0)
        {
            return new ResponseModel
            {
                IsSuccess = false,
                Message = "No completed tasks found in this group"
            };
        }

        var completedTasks = groupTaskList.Where(x => x.IsCompleted).ToList();
        var parentTaskIds = groupTaskList.Select(x => x.TaskId).ToList();

        var completedSubTasks = await subTaskRepo.QueryToGetRecordAsync($"Select * from SubTask where TaskId IN ({string.Join(",", parentTaskIds)}) and IsCompleted =1 and IsDeleted =0").ConfigureAwait(false);

        if ((completedTasks != null && completedTasks.Count() > 0) || (completedSubTasks != null && completedSubTasks.Count() > 0))
        {
            // Delete all Completed subtasks associated with the tasks in this group
            if (completedSubTasks.Any())
            {
                var subTaskIds = string.Join(",", completedSubTasks.Select(x => x.SubTaskId));
                await subTaskRepo.ExecuteSqlAsync($"UPDATE SubTask SET IsDeleted = 1 WHERE SubTaskId IN ({subTaskIds})");
            }

            // Delete all completed tasks associated with this group
            if (completedTasks.Any())
            {
                var taskIdsForDelete = string.Join(",", completedTasks.Select(x => x.TaskId));
                await taskRepo.ExecuteSqlAsync($"UPDATE Task SET IsDeleted = 1 WHERE TaskId IN ({taskIdsForDelete})");
            }
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
        if (group == null)
            return new ResponseModel { IsSuccess = true, Message = "No Record Found." };
        var tasks = await taskRepo?.QueryAsync()?.Where(x => !x.IsDeleted && x.IsStarred && !x.IsCompleted)?.ToList();
        var subTasks = await subTaskRepo?.QueryAsync()?.Where(x => !x.IsDeleted && x.IsStarred && !x.IsCompleted)?.ToList();

        var taskList = tasks?.Select(x => new TaskListVM()
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
        }).ToList() ?? new();

        var subTaskList = subTasks?.Select(x => new TaskListVM()
        {
            SubTaskId = x.SubTaskId,
            TaskId = x.TaskId,
            Title = x.Title,
            Description = x.Description,
            ToDoDate = x.ToDoDate,
            CreateDate = x.CreateDate,
            CompleteDate = x.CompleteDate,
            IsStarred = x.IsStarred,
            IsCompleted = x.IsCompleted,
            TaskGroupId = tasks.FirstOrDefault(x => x.TaskId == x.TaskId).TaskGroupId

        });

        if (subTaskList != null && subTaskList.Count() > 0)
            taskList.AddRange(subTaskList);

        return new()
        {
            IsSuccess = true,
            Message = "Starred tasks retrieved successfully",
            Data = new GroupTaskListVM
            {
                GroupId = group.GroupId,
                GroupName = "Starred tasks",
                SortBy = group.SortBy ?? "My order",
                isEnableShow = true,

                TaskList = taskList?.OrderBy(x => group.SortBy == "Title" ? x.Title :
                                   group.SortBy == "Date" ? (x.ToDoDate ?? DateTime.MaxValue).ToString() :
                                   group.SortBy == "Description" ? x.Description :
                                   x.TaskId.ToString())?.ToList(),
                CompletedTaskList = []
            }
        };
    }

    public async Task<ResponseModel> UpdateVisibilityStatusAsync(int groupId, bool isVisible)
    {
        var group = await taskGroupRepo.GetByIdAsync(groupId);
        if (group == null || group.IsDeleted)
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
        if (group == null || group.IsDeleted)
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
