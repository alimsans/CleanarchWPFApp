﻿using System;
using System.Windows;
using WpfApp1.Controllers;
using WpfApp1.ViewModels;
using WpfApp1.Views;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly TodoListViewModel _todoListViewModel;
        private readonly CrossController _controller;

        public MainWindow()
        {
            InitializeComponent();

            _controller = new CrossController();
            _todoListViewModel = new TodoListViewModel(_controller);
            _todoListViewModel.OnError += TodoListViewModel_OnError;
            _todoListViewModel.BlockingOperationsStarted += OnBlockingOperationsStarted;
            _todoListViewModel.BlockingOperationsFinished += OnBlockingOperationsFinished;
            
            Todo_ListView.ItemsSource = _todoListViewModel.TodoList;
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddTaskWindow();
            window.ShowDialog();

            if (window.TaskModel != null)
                _todoListViewModel.AddTask(window.TaskModel);
        }

        private void GetTaskButton_Click(object sender, RoutedEventArgs e)
        {
            _todoListViewModel.UpdateTodoList();
        }

        private void RemoveTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new RemoveTaskWindow();
            window.ShowDialog();

            if (window.TaskId != -1)
                _todoListViewModel.RemoveTask(window.TaskId);
        }

        #region Event Handlers

        private bool _isSleeping;
        private WaitWindow _waitWindow;
        private void OnBlockingOperationsStarted(object sender, EventArgs e)
        {
            if (!_isSleeping)
            {
                _isSleeping = true;

                Application.Current.Dispatcher?.Invoke(() =>
                {
                    _waitWindow = new WaitWindow()
                    {
                        Owner = this
                    };

                    _waitWindow.ShowDialog();

                    _waitWindow = null;
                });

                _isSleeping = false;
            }
        }

        private void OnBlockingOperationsFinished(object sender, EventArgs e)
        {
            if (_isSleeping)
            {
                 Application.Current.Dispatcher?.Invoke(() => _waitWindow.Close());
            }
        }

        private static void TodoListViewModel_OnError(object sender, System.IO.ErrorEventArgs e)
        {
            var tmp = e.GetException();

            do
            {
                MessageBox.Show(tmp.Message, "ERROR", MessageBoxButton.OK);
                tmp = tmp.InnerException;

            } while (tmp != null);
        }

        #endregion
    }
}
