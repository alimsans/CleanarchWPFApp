using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Cleanarch.DomainLayer.Models;
using Cleanarch.DomainLayer.UseCases;
// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace WpfApp1.Controllers
{
    internal class CrossController
    {
        private IDictionary<Guid, ControllableOperation> _blockingOperations;
        private Queue<ControllableOperation> _nonBlockingOperations;

        public int BlockingOperationsCount => _blockingOperations.Count;
        public int NonBlockingOperationsCount => _nonBlockingOperations.Count;

        public event EventHandler BlockingOperationsStarted; 
        public event EventHandler BlockingOperationsFinished;

        public delegate Task ControllableOperation();

        public CrossController()
        {
            _blockingOperations = new Dictionary<Guid, ControllableOperation>();
            _nonBlockingOperations = new Queue<ControllableOperation>();
        }

        public void AddOperation(ControllableOperation operation, bool isBlocking = false)
        {
            if (isBlocking)
                _blockingOperations.Add(Guid.NewGuid(), operation);
            else
                _nonBlockingOperations.Enqueue(operation);
        }

        public void Execute()
        {
            ExecuteNonBlocking();
            ExecuteBlocking();
        }

        private void ExecuteNonBlocking()
        {
            while (_nonBlockingOperations.Count != 0)
            {
                var operation = _nonBlockingOperations.Dequeue();
                Task.Run(() => operation);
            }
        }

        private void ExecuteBlocking()
        {
            Task.Run(OnBlockingOperationsStarted);

            Task.Run(() =>
            {
                foreach (var guidOperation in _blockingOperations.ToList())
                {
                    guidOperation.Value.Invoke()
                        .ContinueWith(t =>
                        {
                            _blockingOperations.Remove(guidOperation.Key);

                            if(_blockingOperations.Count == 0)
                                OnBlockingOperationsFinished();
                        });
                }
            });
        }

        private void OnBlockingOperationsStarted()
        { 
            BlockingOperationsStarted?.Invoke(this, EventArgs.Empty);
        }

        private void OnBlockingOperationsFinished()
        {
            Task.Run(() => BlockingOperationsFinished?.Invoke(this, EventArgs.Empty));
        }
    }
}
