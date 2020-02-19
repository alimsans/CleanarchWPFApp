using Cleanarch.DomainLayer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cleanarch.DomainLayer.UseCases
{
    public class AddTaskUseCase : UseCase<TaskModel, IEnumerable<TaskModel>> 
    {
        protected override Task<IEnumerable<TaskModel>> GetUseCaseTask() =>
            Repository.AddTaskAsync(Payload);
    }
}
