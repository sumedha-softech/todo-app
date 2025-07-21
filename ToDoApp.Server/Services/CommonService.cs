using ToDoApp.Server.Contracts;
using ToDoApp.Server.Helper;
using ToDoApp.Server.Models;
using ToDoApp.Server.Models.Entity;

namespace ToDoApp.Server.Services
{
    public class CommonService(ICacheService cacheService, IBaseRepository<TaskGroup> taskGroupRepo, IBaseRepository<Models.Entity.Task> taskRepo, IBaseRepository<SubTask> subTaskRepo) : ICommonService
    {
        public async Task<ResponseModel> UndoDeleteItems()
        {
            var cacheModel = cacheService.GetData<UndoDeleteItemCacheModel>(ConstantVariables.CacheKeyForUndoItems);
            if (cacheModel == null)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "No items to undo"
                };
            }
            if (cacheModel.ItemType == "Group")
            {
                var group = await taskGroupRepo.GetByIdAsync(cacheModel.GroupId);
                if (group != null)
                {
                    group.IsDeleted = false;
                    await taskGroupRepo.UpdateAsync(group);
                }
            }
            if (cacheModel.TasksId != null && cacheModel.TasksId.Count > 0)
            {
                var taskIdsForUpdate = string.Join(",", cacheModel.TasksId);
                await taskRepo.ExecuteSqlAsync($"UPDATE Task SET IsDeleted = 0 WHERE TaskId IN ({taskIdsForUpdate})");
            }
            if (cacheModel.SubTasksId != null && cacheModel.SubTasksId.Count > 0)
            {
                var subTaskIdsForUpdate = string.Join(",", cacheModel.SubTasksId);
                await subTaskRepo.ExecuteSqlAsync($"UPDATE SubTask SET IsDeleted = 0 WHERE SubTaskId IN ({subTaskIdsForUpdate})");
            }
            cacheService.RemoveData(ConstantVariables.CacheKeyForUndoItems);
            return new()
            {
                IsSuccess = true,
                Message = "Undo operation completed successfully"
            };
        }

        public async Task<ResponseModel> UndoSubTaskMoved()
        {
            var undoSubTaskCacheModel = cacheService.GetData<UndoSubTaskMovedCacheModel>(ConstantVariables.CacheKeyForUndoSubTaskMoved);
            if (undoSubTaskCacheModel == null)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "No subtask move operation to undo"
                };
            }

            var subTask = await subTaskRepo.GetByIdAsync(undoSubTaskCacheModel.SubTaskId);
            if (subTask == null)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "Subtask not found"
                };
            }
            subTask.IsDeleted = false;
            await subTaskRepo.UpdateAsync(subTask);

            var task = await taskRepo.GetByIdAsync(undoSubTaskCacheModel.TaskId);
            if (task != null)
                await taskRepo.DeleteAsync(task.TaskId);

            if (!undoSubTaskCacheModel.IsExistedGroup)
            {
                var group = await taskGroupRepo.GetByIdAsync(undoSubTaskCacheModel.MovedGroupId);
                if (group != null)
                    await taskGroupRepo.DeleteAsync(group.GroupId);
            }

            cacheService.RemoveData(ConstantVariables.CacheKeyForUndoSubTaskMoved);
            return new ResponseModel
            {
                IsSuccess = true,
                Message = "Subtask move operation undone successfully"
            };

        }

        public async Task<ResponseModel> UndoTaskMoved()
        {
            var cacheModel = cacheService.GetData<UndoTaskMovedCacheModel>(ConstantVariables.CacheKeyForUndoTaskMoved);
            if (cacheModel == null)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "No task move operation to undo"
                };
            }

            var task = await taskRepo.GetByIdAsync(cacheModel.TaskId);
            if (task == null || task.IsDeleted )
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "Task not found"
                };
            }
            task.TaskGroupId = cacheModel.PreviousGroupId;
            await taskRepo.UpdateAsync(task);
            if (!cacheModel.IsExistedGroup)
            {
                var group = await taskGroupRepo.GetByIdAsync(cacheModel.NewGroupId);
                if (group != null)
                    await taskGroupRepo.DeleteAsync(group.GroupId);
            }
            cacheService.RemoveData(ConstantVariables.CacheKeyForUndoTaskMoved);
            return new ResponseModel
            {
                IsSuccess = true,
                Message = "Task move operation undone successfully"
            };
        }
    }
}
