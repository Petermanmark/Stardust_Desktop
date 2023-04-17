using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Test
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void OnLoginButtonClicked(object sender, RoutedEventArgs e)
        {
            // Get the entered username and password
            string username = UsernameTextBox.Text.ToString();
            string password = PasswordBox.Password.ToString();

            // Perform the login check (replace with your own authentication logic)
            bool isLoginSuccessful = false; // replace this with your own authentication logic
            if (isLoginSuccessful)
            {
                // Redirect the user to the main window
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                Close();
            }
            else
            {
                // Display an error message
                ErrorMessageTextBlock.Text = "Invalid username or password.";
            }
        }
    }
}
