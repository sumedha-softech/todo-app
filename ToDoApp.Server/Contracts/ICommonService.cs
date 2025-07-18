using ToDoApp.Server.Models;

namespace ToDoApp.Server.Contracts
{
    public interface ICommonService
    {
        Task<ResponseModel> UndoDeleteItems();
        Task<ResponseModel> UndoSubTaskMoved();
        Task<ResponseModel> UndoTaskMoved();
    }
}
