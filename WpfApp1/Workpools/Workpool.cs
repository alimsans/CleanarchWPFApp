using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cleanarch.DomainLayer.Models;
using Cleanarch.DomainLayer.UseCases;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable CollectionNeverQueried.Global

namespace WpfApp1.Workpools
{
    internal class Workpool
    {
        private readonly UseCase<TaskModel, object>[] _useCases;

        public int Count { get; }
        public int PendingCount { get; private set; }
        public int StartedCount { get; private set; }

        public object Result { get; private set; }
        public ICollection<Exception> Exceptions { get; private set; }
        public UseCaseHandler<object> Handler {get; private set; }

        public Workpool(UseCaseHandler<object> handler = null, params UseCase<TaskModel, object>[] useCases)
        {
            _useCases = useCases;
            Handler = handler;
            Count = PendingCount = useCases.Length;

            Result = new List<TaskModel>();
            Exceptions = new List<Exception>();
        }

        public Workpool(params UseCase<TaskModel, object>[] useCases)
            : this(null, useCases)
        {
        }

        public void Execute(UseCaseHandler<object> handler = null)
        {
            if (handler != null) 
                Handler = handler;

            Handler?.OnStart?.Invoke();

            var tasks = new List<Task>();

            foreach (var useCase in _useCases)
            {
                var currentHandler = useCase.Execute(OnComplete, OnError, OnStart);
                tasks.Add(currentHandler.ExecutingTask);
            }

            Task.WaitAll(tasks.ToArray());

            Handler?.OnComplete?.Invoke(Result);
        }

        private void OnStart()
        {
            StartedCount++;
        }

        private void OnComplete(object result)
        {
            PendingCount--;
            Result = result;
        }

        private void OnError(Exception e)
        {
            Exceptions.Add(e);
            PendingCount--;
            Handler?.OnError?.Invoke(e);
        }
    }
}
