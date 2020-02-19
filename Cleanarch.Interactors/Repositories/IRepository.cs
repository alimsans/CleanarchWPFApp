using System.Collections.Generic;
using System.Threading.Tasks;
using Cleanarch.DomainLayer.Models;

namespace Cleanarch.DomainLayer.Repositories
{
    internal interface IRepository
    {
        Task<TaskModel> GetTaskAsync(int id);
        Task<IEnumerable<TaskModel>> GetAllTasksAsync();
        Task<IEnumerable<TaskModel>> AddTaskAsync(TaskModel model);
        Task<IEnumerable<TaskModel>> AddAllTasksAsync(IEnumerable<TaskModel> models);
        Task<IEnumerable<TaskModel>> RemoveTaskAsync(TaskModel model);
    }
}
