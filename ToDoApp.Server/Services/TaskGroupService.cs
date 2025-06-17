using ToDoApp.Server.Contracts;
using ToDoApp.Server.Models;
using ToDoApp.Server.Models.Entity;

namespace ToDoApp.Server.Services;

public class TaskGroupService(IBaseRepository<TaskGroup> taskGroupRepo, IBaseRepository<Models.Entity.Task> taskRepo) : ITaskGroupService
{
    public async Task<ResponseModel> GetTaskGroupsAsync()
    {
        ResponseModel response = new();
        try
        {
            response.Data = await taskGroupRepo.GetAllAsync().ConfigureAwait(false);
            response.IsSuccess = true;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
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

    public async Task<ResponseModel> AddOrUpdateTaskGroupAsync(int id, TaskGroup model)
    {
        ResponseModel response = new();
        try
        {
            if (id == 0)
            {
                model.ListId = 0;
                await taskGroupRepo.AddAsync(model);
                response.Message = "Task Group added successfully";
            }
            else
            {
                model.ListId = id;
                await taskGroupRepo.UpdateAsync(model);
                response.Message = "Task Group updated successfully";
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

    public async Task<ResponseModel> DeleteGroupAsync(int id)
    {
        ResponseModel response = new();
        try
        {
            var tasks = await taskRepo.QueryNewAsync().Where(x => x.TaskGroupId == id).ToList();
            foreach (var item in tasks)
            {
                try
                {
                    await taskRepo.DeleteAsync(item.TaskId);
                }
                catch (Exception)
                {
                    // ToDo handle error;
                }
            }

            await taskGroupRepo.DeleteAsync(id);
            response.IsSuccess = true;
            response.Message = "Task Group deleted successfully";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }
}
