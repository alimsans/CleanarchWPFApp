using System;
using Autofac;
using System.Threading.Tasks;
using Cleanarch.DomainLayer.Models;
using Cleanarch.DomainLayer.Repositories;

namespace Cleanarch.DomainLayer.UseCases
{

    public abstract class UseCase<TInput, TOutput>
        where TInput: UseCasePayload
    {
        public TInput Payload { get; set; }
        public Task ExecutingTask { get; private set; }

        internal readonly IRepository Repository;

        
        protected internal UseCase()
        {
            Repository = Configurer.Container.Resolve<IRepository>();
        }

        public UseCaseHandler<TOutput> Execute(UseCaseHandler<TOutput>.OnCompleteCallback<TOutput> onComplete = null,
            UseCaseHandler<TOutput>.OnErrorCallback onError = null,
            UseCaseHandler<TOutput>.OnStartCallback onStart = null)
        {
            var handler = new UseCaseHandler<TOutput>()
            {
                OnComplete = onComplete,
                OnError = onError,
                OnStart = onStart
            };

            return Execute(handler);
        }

        public UseCaseHandler<TOutput> Execute(UseCaseHandler<TOutput> handler)
        {
            ExecutingTask = Task.Run(async () =>
            {
                handler.OnStart?.Invoke();

                try
                {
                    var result = await GetUseCaseTask();

                    //TODO: remove this.
                    await Task.Delay(3000);

                    handler.Result = result;
                    handler.OnComplete?.Invoke(result);
                }
                catch (Exception e)
                {
                    handler.OnError?.Invoke(e);
                }
            });

            handler.ExecutingTask = ExecutingTask;

            return handler;
        }

        protected abstract Task<TOutput> GetUseCaseTask();
    }
}
