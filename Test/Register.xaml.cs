using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace Test
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public Register()
        {
            InitializeComponent();
            this.ResizeMode = ResizeMode.NoResize;
        }
        private void closeApp(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Back(object sender, RoutedEventArgs e)
        {
            var loginb = new Login(); //create your new form.
            loginb.Show(); //show the new form.
            this.Close(); //only if you want to close the current form.
        }
        private async void OnRegisterButtonClicked(object sender, RoutedEventArgs e)
        {
            // Get the entered email and password
            string email = EmailTextBox.Text.ToString();
            string password = PasswordBox.Password.ToString();

            // Validate the email and password fields
            if (string.IsNullOrEmpty(email) || !IsValidEmail(email))
            {
                MessageBox.Show("Please enter a valid email address");
                return;
            }
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter a password");
                return;
            }

            // Perform the user registration using the backend API
            try
            {
                string result = await PerformUserRegistration(email, password);

                // Display a success message
                MessageBox.Show(result);

                // Redirect the user to the login window
                Login loginWindow = new Login();
                loginWindow.Show();
                Close();
            }
            catch (Exception ex)
            {
                // Display an error message
                MessageBox.Show(ex.Message);
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }


        private async Task<string>PerformUserRegistration(string email, string password)
        {
            // Create an HTTP client object
            using (HttpClient client = new HttpClient())
            {
                // Set the base URL for the API
                client.BaseAddress = new Uri("http://localhost:3000");

                // Create a JSON object with the username, email, and password
                UserRegister user = new UserRegister();
                user.email = email;
                user.password = password;
                var json = JsonConvert.SerializeObject(user);
                Trace.WriteLine(user);

                // Create an HTTP request message with the JSON object in the body
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("/auth/signup", content);

                // If the response was successful, return the success message
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return "Register successful";
                }
                else
                {
                    // If the response was not successful, throw an exception with the error message
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception(error);
                }
            }
        }
    }
}
