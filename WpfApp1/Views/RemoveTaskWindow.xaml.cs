using System.Windows;

namespace WpfApp1.Views
{
    /// <summary>
    /// Interaction logic for RemoveTaskWindow.xaml
    /// </summary>
    public partial class RemoveTaskWindow : Window
    {
        public int TaskId { get; private set; }

        public RemoveTaskWindow()
        {
            InitializeComponent();
        }

        private void Submit_Button_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(this.TaskId_TextBox.Text, out var taskId))
                TaskId = taskId;
            else
                TaskId = -1;

            this.Close();
        }
    }
}
