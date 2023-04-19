using MaterialDesignThemes.Wpf;
using Microsoft.Azure.Cosmos.Serialization.HybridRow.RecordIO;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            this.ResizeMode = ResizeMode.NoResize;
            getProfile(emailTextWin, registerTextWin);
            getNotes(stackPanel);
        }
        private void minimalizeApp(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void closeApp(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void newNote(object sender, RoutedEventArgs e)
        {
            newNoteDialog dialog = new newNoteDialog();
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                // The user clicked the "OK" button
                string text1 = dialog.textBox1.Text;
                string text2 = dialog.textBox2.Text;
                // Do something with the values here
                newNotePost(text1, text2);
                stackPanel.Children.Clear();
                getNotes(stackPanel);
            }
            else
            {
                // The user clicked the "Cancel" button or closed the dialog box
            }
        }
        void newGod(object sender, RoutedEventArgs e)
        {
            newGodPost();
            stackPanel.Children.Clear();
            getNotes(stackPanel);
        }
        private void logout(object sender, RoutedEventArgs e)
        {
            Token.accessToken = "";
            var login = new Login();
            login.Show();
            this.Close();
        }
        public async void getNotes(StackPanel stackPanel)
        {
            // Create an HTTP client object
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer "+Token.accessToken);
                var response = await client.GetAsync("/notes");

                // If the response was successful, return the success message
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    dynamic data = JsonConvert.DeserializeObject(json);

                    // Define the stack panel that will contain the cards
                    stackPanel.Margin = new Thickness(10);;

                    // Assume that the API data is an array of objects with "name" property
                    foreach (dynamic item in data)
                    {

                        // Define the card data
                        string noteId = item.noteId;
                        string title = item.title;
                        string content = item.content;

                        // Create the card
                        Card card = new Card();
                        card.BorderBrush = System.Windows.Media.Brushes.Black;
                        card.BorderThickness = new Thickness(1);
                        card.Margin = new Thickness(5);
                        card.Padding = new Thickness(15);

                        Grid gridCard = new Grid();
                        gridCard.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                        gridCard.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                        gridCard.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                        TextBlock titleCard = new TextBlock();
                        titleCard.Text = title;
                        titleCard.FontSize = 20;
                        titleCard.FontWeight = System.Windows.FontWeights.Bold;
                        titleCard.Margin = new Thickness(5);
                        titleCard.TextWrapping = TextWrapping.Wrap;
                        Grid.SetRow(titleCard, 0);
                        Grid.SetColumn(titleCard, 1);

                        TextBlock contentCard = new TextBlock();

                        contentCard.Text = content;
                        contentCard.FontSize = 16;
                        contentCard.Margin = new Thickness(5);
                        contentCard.TextWrapping = TextWrapping.Wrap;
                        Grid.SetRow(contentCard, 1);
                        Grid.SetColumn(contentCard, 1);

                        // Bind the TextBlocks' Width property to the parent element's ActualWidth
                        titleCard.SetBinding(TextBlock.WidthProperty, new Binding("ActualWidth") { Source = card });
                        contentCard.SetBinding(TextBlock.WidthProperty, new Binding("ActualWidth") { Source = card });

                        // Wrap the TextBlocks in a Viewbox to make them responsive
                        Viewbox titleViewbox = new Viewbox();
                        titleViewbox.Child = titleCard;

                        Viewbox contentViewbox = new Viewbox();
                        contentViewbox.Child = contentCard;

                        // Add the Viewboxes to the grid
                        Grid.SetRow(titleViewbox, 0);
                        Grid.SetColumn(titleViewbox, 1);
                        gridCard.Children.Add(titleViewbox);

                        Grid.SetRow(contentViewbox, 1);
                        Grid.SetColumn(contentViewbox, 1);
                        gridCard.Children.Add(contentViewbox);

                        // Create a StackPanel for the buttons
                        StackPanel buttonPanel = new StackPanel();
                        buttonPanel.Orientation = Orientation.Horizontal;
                        buttonPanel.Margin = new Thickness(5);
                        Grid.SetRow(buttonPanel, 2);
                        Grid.SetColumn(buttonPanel, 1);

                        // Create the buttons and add them to the StackPanel
                        Button editButton = new Button();
                        editButton.Content = "Edit";
                        editButton.Tag = noteId;
                        editButton.Margin = new Thickness(5);
                        editButton.Click += EditButton_Click;
                        buttonPanel.Children.Add(editButton);
                        void EditButton_Click(object sender, RoutedEventArgs e)
                        {
                            // retrieve the note ID from the Tag property of the button
                            string noteId = (string)((Button)sender).Tag;
                            newNoteDialog dialog = new newNoteDialog();
                            bool? result = dialog.ShowDialog();
                            if (result == true)
                            {
                                // The user clicked the "OK" button
                                string text1 = dialog.textBox1.Text;
                                string text2 = dialog.textBox2.Text;
                                // Do something with the values here
                                editNote(noteId, text1, text2);
                                stackPanel.Children.Clear();
                                getNotes(stackPanel);
                            }
                            else
                            {
                                // The user clicked the "Cancel" button or closed the dialog box
                            }

                        }

                        Button deleteButton = new Button();
                        deleteButton.Content = "Delete";
                        deleteButton.Tag = noteId;
                        deleteButton.Click += DeleteButton_Click;
                        deleteButton.Margin = new Thickness(5);
                        buttonPanel.Children.Add(deleteButton);
                        void DeleteButton_Click(object sender, RoutedEventArgs e)
                        {
                            // retrieve the note ID from the Tag property of the button
                            string noteId = (string)((Button)sender).Tag;
                            deleteNote(noteId);
                            stackPanel.Children.Clear();
                            getNotes(stackPanel);

                        }

                        gridCard.Children.Add(buttonPanel);

                        card.Content = gridCard;
                        stackPanel.Children.Add(card);
                    }
                }

                else
                {
                    // If the response was not successful, throw an exception with the error message
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception(error);
                }
            }
        }
        public async void getProfile(MenuItem emailTextWin, MenuItem registerTextWin)
        {
            // Create an HTTP client object
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token.accessToken);
                var response = await client.GetAsync("/profile");

                // If the response was successful, return the success message
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    dynamic data = JsonConvert.DeserializeObject(json);
                    // Define the card data
                    string email = data.email;
                    DateTime registartionDate = data.registartionDate;

                    emailTextWin.Header = email;
                    registerTextWin.Header = "Regisztráció:\n"+registartionDate;

                    }

                else
                {
                    // If the response was not successful, throw an exception with the error message
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception(error);
                }
            }
        }
        public void newGodPost()
        {
            // create HTTP client instance
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token.accessToken);
            // create HTTP request message
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:3000/notes/god");
            // send HTTP request
            HttpResponseMessage response = client.SendAsync(request).Result;
            // check if HTTP response was successful
            if (response.IsSuccessStatusCode)
            {
                Trace.WriteLine($"{response.StatusCode}");
            }
            else
            {
                Trace.WriteLine($"{response.StatusCode}");
            }
        }
        public void newNotePost(string title, string content)
        {
            // create HTTP client instance
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token.accessToken);
            // create HTTP request message
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:3000/notes/");
            // Create a JSON object with the username, email, and password
            NewNote newNote = new NewNote();
            newNote.title = title;
            newNote.content = content;
            var json = JsonConvert.SerializeObject(newNote);
            // set the request content to a JSON payload
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            // send HTTP request
            HttpResponseMessage response = client.SendAsync(request).Result;
            // check if HTTP response was successful
            if (response.IsSuccessStatusCode)
            {
                Trace.WriteLine($"{response.StatusCode}");
            }
            else
            {
                Trace.WriteLine($"{response.StatusCode}");
            }
        }
        public void deleteNote(string noteId)
        {
            // create HTTP client instance
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token.accessToken);
            // create HTTP request message
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, "http://localhost:3000/notes/"+noteId);
            // send HTTP request
            HttpResponseMessage response = client.SendAsync(request).Result;
            // check if HTTP response was successful
            if (response.IsSuccessStatusCode)
            {
                Trace.WriteLine($"{response.StatusCode}");
            }
            else
            {
                Trace.WriteLine($"{response.StatusCode}");
            }
        }
        
        //EZ BAD REQUESTET DOB
        public void editNote(string noteId, string title, string content)
        {
            // create HTTP client instance
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token.accessToken);
            // create HTTP request message
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Patch, "http://localhost:3000/notes/" + noteId);
            // create a JSON object with the data to update
            var updateData = new
            {
                title = title,
                content = content
            };          
            var json = JsonConvert.SerializeObject(updateData);
            // set the request content to a JSON payload
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            Trace.WriteLine($"{json}");
            Trace.WriteLine($"{noteId}");
            // send HTTP request
            HttpResponseMessage response = client.SendAsync(request).Result;
            // check if HTTP response was successful
            if (response.IsSuccessStatusCode)
            {
                Trace.WriteLine($"{response.StatusCode}");
            }
            else
            {
                Trace.WriteLine($"{response.StatusCode}");
            }
        }
    }
}
