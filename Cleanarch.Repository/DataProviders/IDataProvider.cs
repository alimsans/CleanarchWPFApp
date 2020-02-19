using System.Collections.Generic;
using System.Threading.Tasks;
using Cleanarch.Repository.Entities;


namespace Cleanarch.Repository.DataProviders
{
    public interface IDataProvider
    {
        /// <summary>
        /// Gets a single <see cref="TaskEntity"/> by id.
        /// </summary>
        /// <returns>NULL if not found.</returns>
        Task<TaskEntity> GetTaskAsync(int id);

        /// <summary>
        /// Gets all stored <see cref="TaskEntity"/>.
        /// </summary>
        /// <returns>NULL if no <see cref="TaskEntity"/> exists.</returns>
        Task<IEnumerable<TaskEntity>> GetAllTasksAsync();

        /// <summary>
        /// Adds a task
        /// </summary>
        /// <returns>True on success.</returns>
        Task<IEnumerable<TaskEntity>> AddTaskAsync(TaskEntity entity);

        /// <summary>
        /// Adds all tasks
        /// </summary>
        /// <returns>True on success.</returns>
        Task<IEnumerable<TaskEntity>> AddAllTasksAsync(IEnumerable<TaskEntity> entities);

        Task<IEnumerable<TaskEntity>> RemoveTaskAsync(TaskEntity entity);
    }
}
