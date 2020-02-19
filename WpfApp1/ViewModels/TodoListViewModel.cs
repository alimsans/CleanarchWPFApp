using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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

        public event EventHandler BlockingOperationsStarted;
        public event EventHandler BlockingOperationsFinished;

        public TodoListViewModel()
        {
            TodoList = new ObservableCollection<TaskModel>();

            _controller = new CrossController();
            _controller.BlockingOperationsStarted += (sender, e) => BlockingOperationsStarted?.Invoke(sender, e);
            _controller.BlockingOperationsFinished += (sender, e) => BlockingOperationsFinished?.Invoke(sender, e);

            _useCaseHandler = new UseCaseHandler<IEnumerable<TaskModel>>();
        }

        public void AddTask(TaskModel taskModel)
        {
            var useCase = new AddTaskUseCase { Payload = taskModel };
           
            _controller.AddOperation(() => useCase.ExecuteInController(_useCaseHandler), true);
            _controller.Execute();
        }

        private void UpdateTodoList(IEnumerable<TaskModel> tasks)
        {
            Application.Current.Dispatcher?.Thread.Interrupt();
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

                _controller.AddOperation(() => useCase.ExecuteInController(_useCaseHandler), true);
                _controller.Execute();
            }
        }

        public void UpdateTodoList()
        {
            var useCase = new GetTasksUseCase();

            _controller.AddOperation(() => useCase.ExecuteInController(_useCaseHandler), true);
            _controller.Execute();
        }

        #region Event notifications

        private void NotifyOnError(Exception e)
        {
            OnError?.Invoke(this, new ErrorEventArgs(e));
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
