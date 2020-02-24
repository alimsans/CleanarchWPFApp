using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Threading.Tasks;
using System.Windows;
using Cleanarch.DomainLayer.Models;
using Cleanarch.DomainLayer.UseCases;
using WpfApp1.Controllers;

namespace WpfApp1.ViewModels
{
    public class TodoListViewModel : INotifyPropertyChanged
    {
        private readonly CrossController _controller;
        private readonly UseCaseHandler<IEnumerable<TaskModel>> _useCaseHandler;

        public ObservableCollection<TaskModel> TodoList { get; }
        public event PropertyChangedEventHandler PropertyChanged;
        public event ErrorEventHandler OnError;

        private event EventHandler OperationFinished;
        public event EventHandler BlockingOperationsStarted;
        public event EventHandler BlockingOperationsFinished;

        internal TodoListViewModel(CrossController controller)
        {
            TodoList = new ObservableCollection<TaskModel>();

            _controller = controller;
            _controller.ControlBlocked += OnBlockingOperationsStarted;
            _controller.ControlFreed += OnBlockingOperationsFinished;

            _useCaseHandler = new UseCaseHandler<IEnumerable<TaskModel>>(
                onComplete: OnUseCaseComplete,
                onError: e =>  OnError?.Invoke(this, new ErrorEventArgs(e)));
        }

        private void OnUseCaseComplete(IEnumerable<TaskModel> data)
        {
            UpdateTodoList(data);
            OnOperationFinished();
        }

        public void AddTask(TaskModel taskModel)
        {
            var useCase = new AddTaskUseCase { Payload = taskModel };

            _controller.RegisterOperation(ref OperationFinished);

            useCase.Execute(_useCaseHandler);
        }

        private void UpdateTodoList(IEnumerable<TaskModel> tasks)
        {
            Application.Current.Dispatcher?.Invoke(() =>
            {
                TodoList.Clear();

                foreach (var taskModel in tasks)
                {
                    TodoList.Add(taskModel);
                }
            });
        }

        public void RemoveTask(int id)
        {
            var taskModel = TodoList.FirstOrDefault(x => x.Id == id);

            if (taskModel != null)
            {
                var useCase = new RemoveTaskUseCase { Payload = taskModel };

                _controller.RegisterOperation(ref OperationFinished);
            }
        }

        public void UpdateTodoList()
        {
            var useCase = new GetTasksUseCase();

            useCase.Execute(_useCaseHandler);

            _controller.RegisterOperation(ref OperationFinished);
        }

        private void OnBlockingOperationsStarted(object sender, EventArgs e)
        {
            Task.Run(() => BlockingOperationsStarted?.Invoke(this, EventArgs.Empty));
        }

        private void OnBlockingOperationsFinished(object sender, EventArgs e)
        {
            BlockingOperationsFinished?.Invoke(this, EventArgs.Empty);
        }

        protected void OnOperationFinished()
        {
            OperationFinished?.Invoke(this, EventArgs.Empty);
        }
    }
}
