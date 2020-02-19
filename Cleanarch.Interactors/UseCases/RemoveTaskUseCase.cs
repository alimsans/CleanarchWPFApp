using System.Collections.Generic;
using Cleanarch.DomainLayer.Models;
using System.Threading.Tasks;

namespace Cleanarch.DomainLayer.UseCases
{
    public class RemoveTaskUseCase : UseCase<TaskModel, IEnumerable<TaskModel>>
    {
        protected override Task<IEnumerable<TaskModel>> GetUseCaseTask() =>
            Repository.RemoveTaskAsync(Payload);
    }
}
