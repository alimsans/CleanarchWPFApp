using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cleanarch.Repository.Entities;
using Newtonsoft.Json;

namespace Cleanarch.Repository.DataProviders
{
    public class LocalDataProvider : IDataProvider
    {
        private readonly string _path;

        public LocalDataProvider(string path)
        {
            _path = path;

            if (!File.Exists(path))
                File.Create(path);
        }

        public async Task<TaskEntity> GetTaskAsync(int id)
        {
            var entities = await GetAllTasksAsync();

            return entities?.FirstOrDefault(x => x.Id == id);
        }

        public async Task<IEnumerable<TaskEntity>> GetAllTasksAsync()
        {
            string data;
            using (var reader = new StreamReader(_path))
            {
                data = await reader.ReadToEndAsync();
            }

            var entities = JsonConvert.DeserializeObject<List<TaskEntity>>(data);

            return entities;
        }

        public async Task<IEnumerable<TaskEntity>> AddTaskAsync(TaskEntity entity)
        {
            var newEntities = await AddAllTasksAsync(new[] { entity });

            return newEntities;
        }

        public async Task<IEnumerable<TaskEntity>> AddAllTasksAsync(IEnumerable<TaskEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            int currentId = 1;

            var readEntities = await GetAllTasksAsync();

            if (readEntities == null || !readEntities.Any())
                readEntities = new List<TaskEntity>(1);
            else
                currentId = readEntities.Last().Id + 1;

            var tasks = readEntities as List<TaskEntity>;

            foreach (var entity in entities)
            {
                entity.Id = currentId;
                tasks.Add(entity);

                currentId++;
            }

            await WriteEntities(tasks);
            return tasks;
        }

        public async Task<IEnumerable<TaskEntity>> RemoveTaskAsync(TaskEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var readEntities = await GetAllTasksAsync();
            if (readEntities == null)
                return readEntities;

            var writeEntities = readEntities.Where(x => x.Id != entity.Id);

            await WriteEntities(writeEntities);

            return writeEntities;
        }

        private async Task WriteEntities(IEnumerable<TaskEntity> entities)
        {
            var content = JsonConvert.SerializeObject(entities);

            using (var writer = new StreamWriter(_path))
            {
                await writer.WriteAsync(content);
            }
        }
    }
}
