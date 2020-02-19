using Cleanarch.DomainLayer.Models;
using System;
using System.Windows;

namespace WpfApp1.Views
{
    /// <summary>
    /// Interaction logic for AddTaskWindow.xaml
    /// </summary>
    public partial class AddTaskWindow : Window
    {
        public TaskModel TaskModel { get; private set; }

        public AddTaskWindow()
        {
            InitializeComponent();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.Title_TextBox.Text))
            {
                MessageBox.Show("Title cannot be empty.", "ERROR", MessageBoxButton.OK);
                return;
            }

            if (string.IsNullOrWhiteSpace(this.Desciption_TextBox.Text))
            {
                MessageBox.Show("Description cannot be empty.", "ERROR", MessageBoxButton.OK);
                return;
            }

            if (!DateTimeOffset.TryParse(this.Date_TextBox.Text, out var dateTime))
            {
                MessageBox.Show("Invalid Date format.", "ERROR", MessageBoxButton.OK);
                return;
            }

            TaskModel = new TaskModel(Title_TextBox.Text, dateTime, Desciption_TextBox.Text);

            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
