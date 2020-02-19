using System.Collections.Generic;
using System.Threading.Tasks;
using Cleanarch.DomainLayer.Models;
using Cleanarch.Repository.DataProviders;

namespace Cleanarch.DomainLayer.Repositories
{
    internal class Repository : IRepository
    {
        private readonly IDataProvider _dataProvider;

        public Repository(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public async Task<TaskModel> GetTaskAsync(int id)
        {
            var taskEntity = await _dataProvider.GetTaskAsync(id);

            return taskEntity.ToTaskModel();
        }

        public async Task<IEnumerable<TaskModel>> GetAllTasksAsync()
        {
            var taskEntities = await _dataProvider.GetAllTasksAsync();

            return taskEntities.ToTaskModels();
        }

        public async Task<IEnumerable<TaskModel>> AddTaskAsync(TaskModel model)
        {
            var newTaskEntities = await _dataProvider.AddTaskAsync(model.ToTaskEntity());

            return newTaskEntities.ToTaskModels();
        }

        public async Task<IEnumerable<TaskModel>> AddAllTasksAsync(IEnumerable<TaskModel> models)
        {
            var newTaskEntities = await _dataProvider.AddAllTasksAsync(models.ToTaskEntities());

            return newTaskEntities.ToTaskModels();
        }

        public async Task<IEnumerable<TaskModel>> RemoveTaskAsync(TaskModel model)
        {
            var newTaskEntities = await _dataProvider.RemoveTaskAsync(model.ToTaskEntity());

            return newTaskEntities.ToTaskModels();
        }
    }
}
