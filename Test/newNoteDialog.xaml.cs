using System.Windows;

namespace Test
{
    /// <summary>
    /// Interaction logic for newNoteDialog.xaml
    /// </summary>
    public partial class newNoteDialog : Window
    {
        public newNoteDialog()
        {
            InitializeComponent();
            this.ResizeMode = ResizeMode.NoResize;
        }
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            string title = textBox1.Text;
            string content = textBox2.Text;
            // Pass the values back to the parent window here
            this.DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
