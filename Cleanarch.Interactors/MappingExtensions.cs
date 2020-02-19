using System.Collections.Generic;
using System.Linq;
using Cleanarch.DomainLayer.Models;
using Cleanarch.Repository.Entities;

namespace Cleanarch.DomainLayer
{
    internal static class MappingExtensions
    {
        public static TaskModel ToTaskModel(this TaskEntity entity)
        {
            return new TaskModel(entity.Title, entity.Date, entity.Description)
            {
                Id = entity.Id,
                IsComplete = entity.IsComplete
            };
        }
        public static TaskEntity ToTaskEntity(this TaskModel model)
        {
            return new TaskEntity(model.Title, model.Date, model.Description)
            {
                Id = model.Id,
                IsComplete = model.IsComplete
            };
        }

        public static IEnumerable<TaskModel> ToTaskModels(this IEnumerable<TaskEntity> entities)
        {
            return entities.Select(entity => entity.ToTaskModel()).ToList();
        }

        public static IEnumerable<TaskEntity> ToTaskEntities(this IEnumerable<TaskModel> models)
        {
            return models.Select(model => model.ToTaskEntity()).ToList();
        }
    }
}
