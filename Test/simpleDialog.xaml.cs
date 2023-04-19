using System.Windows;

namespace Test
{
    /// <summary>
    /// Interaction logic for simpleDialog.xaml
    /// </summary>
    public partial class simpleDialog : Window
    {
        public simpleDialog()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
