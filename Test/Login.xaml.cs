using Azure.Storage.Blobs.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
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
    //sussy baka
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            this.ResizeMode = ResizeMode.NoResize;
        }
        private void closeApp(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void toRegister(object sender, RoutedEventArgs e)
        {
            var registernb = new Register(); //create your new form.
            registernb.Show(); //show the new form.
            this.Close(); //only if you want to close the current form.
        }
        private async void OnLoginButtonClicked(object sender, RoutedEventArgs e)
        {
            // Get the entered username and password
            string email = EmailTextBox.Text.ToString();
            string password = PasswordBox.Password.ToString();

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

            try
            {
                string result = await PerformUserLogin(email, password);

                // Display a success message
                MessageBox.Show(result);

                // Redirect the user to the login window
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
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

        private async Task<string> PerformUserLogin(string email, string password)
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
                var response = await client.PostAsync("/auth/login", content);

                // If the response was successful, return the success message
                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    JObject jsonObject = JObject.Parse(jsonString);
                    Token.accessToken = (string)jsonObject["access_token"];

                    return "Login Successful";
                }
                else
                {
                    // If the response was not successful, throw an exception with the error message
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception(error);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
